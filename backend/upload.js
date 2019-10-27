var admin = require("firebase-admin");
const csv = require('csv-parser');
const fs = require('fs');
var Firestore = require('@google-cloud/firestore');

// Fetch the service account key JSON file contents
var serviceAccount = require("./tracksy-5a1ad-firebase-adminsdk-8qetz-634712679e.json");

// Initialize the app with a service account, granting admin privileges
admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: "https://tracksy-5a1ad.firebaseio.com"
});

// As an admin, the app has access to read and write all data, regardless of Security Rules
var db = admin.firestore();
let writeBatch = db.batch();

const uploader = (jsonData) => {
    let docRef = db.collection('log').doc();

    writeBatch.set(
        docRef,
        {
            'name': jsonData["individual-local-identifier"],
            'localId': jsonData["tag-local-identifier"],
            'location': new Firestore.GeoPoint(parseFloat(jsonData["location-lat"]), parseFloat(jsonData['location-long'])),
            'time': Firestore.Timestamp.fromMillis(Date.parse(jsonData['timestamp']))
        }
    );
};

let counter = 0;
const csvStream = fs.createReadStream('./logs.csv').pipe(csv());

csvStream.on(
    'data',
    (data) => {
        uploader(data);
        counter++;
        if (counter > 200) {
            csvStream.pause();
            writeBatch.commit().then(
                () => {
                    writeBatch = db.batch();
                    counter = 0;
                    csvStream.resume();
                }
            );
        }
    }
).on('end', () => {
    writeBatch.commit().then(() => {
        console.log('Successfully executed batch.');
    });
});

// db.collection('log').get()
//     .then((snapshot) => {
//         snapshot.forEach((doc) => {
//             console.log(doc.id, '=>', doc.data());
//         });
//     })
//     .catch((err) => {
//         console.log('Error getting documents', err);
//     });
