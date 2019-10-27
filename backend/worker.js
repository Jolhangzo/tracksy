var admin = require("firebase-admin");
var Firestore = require('@google-cloud/firestore');
var CronJob = require('cron').CronJob;
var moment = require('moment');
const axios = require('axios');

var serviceAccount = require("./tracksynew-firebase-adminsdk-xrgal-0e3d31de18");

// Initialize the app with a service account, granting admin privileges
admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: "https://tracksynew.firebaseio.com"
});

// As an admin, the app has access to read and write all data, regardless of Security Rules
var db = admin.firestore();

const places = [];

const getRandomInt = (max) => {
    return Math.floor(Math.random() * Math.floor(max));
};

const getSign = () => getRandomInt(2) ? -1 : 1;

const runner = (id) => {
    let oldp;
    let newp;

    if (places.length > 19) {
        const offsets = [
            getSign(),
            getSign()
        ].map((i) => i * getRandomInt(5000) / 10000);

        oldp = places[places.length-1];

        newp = {good: getRandomInt(1), lat: oldp.lat + offsets[0], lng: oldp.lng + offsets[1]};
    } else {
        const offsets = [
            getSign(),
            getSign()
        ].map((i) => i * getRandomInt(500) / 10000);

        oldp = getRandomInt(1) ? places[places.length-1] : places.filter((p) => p.good).reduce((acc, c) => getRandomInt(1) ? acc : c);

        newp = {good: true, lat: oldp.lat + offsets[0], lng: oldp.lng + offsets[1]}
    }

    places.push(newp);

    const when = moment().add(places.length * 5, 'h');

    axios.post(
        'https://us-central1-tracksynew.cloudfunctions.net/log/' + id,
        {
            animalId: parseInt(id),
            coordinate: {lat: newp.lat, lng: newp.lng},
            dateTime: when.valueOf(),
            dateTimeValue: when.format()
        }
    );

    // console.log({
    //     coordinate: {lat: newp.lat, lng: newp.lng},
    //     dateTime: when.valueOf(),
    //     dateTimeValue: when.format()
    // });
};


let [_, __, name, id, lat, lng, bd] = process.argv;

lat = parseFloat(lat);
lng = parseFloat(lng);

axios.post(
    'https://us-central1-tracksynew.cloudfunctions.net/animal/' + id,
    {
        name,
        animalId: parseInt(id),
        coordinate: {lat, lng},
        birthdateValue: bd,
        birthdate: moment(bd).unix(),
        photoUrls: [
        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/52929241%403x.png?alt=media&token=8f253b1a-ff8f-445a-8ce3-71ec98f84aa5",
        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpyEagle%403x.png?alt=media&token=bdac1e65-c4da-4896-98a6-5347fb0c2d61",
        "https://firebasestorage.googleapis.com/v0/b/tracksy-5a1ad.appspot.com/o/harpy%403x.png?alt=media&token=18c7dd73-947d-4483-a0af-57cc512cfb1e"
        ],
        type: "Harpy Eagle",
        imageUrl: "bird_marker3",
        location: "Hungary"
    }
);

places.push({good: true, lat, lng});

const futoka = new CronJob(
    // '30 */10 * * * *',
    '*/30 * * * * *',
    () => runner(id)
);

futoka.start();
