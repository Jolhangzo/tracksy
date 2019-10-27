const functions = require('firebase-functions');
const admin = require('firebase-admin');
const express = require('express');

const {GeoFirestore} = require('geofirestore');


const appAnimal = express();
const appLog = express();

admin.initializeApp();

const firestore = admin.firestore();
const geofirestore = new GeoFirestore(firestore);

const geoAnimal = geofirestore.collection('animal');

appLog.get(
    '/:id',
    (req, res) => {
        const id = parseInt(req.params.id);
        firestore.collection('log')
            .where('animalId', '==', id)
            .limit(50)
            .get()
            .then(
                snapshot => {
                    if (snapshot.empty) {
                        res.status(204).end();
                        return;
                    }

                    return res.status(200).end(JSON.stringify(snapshot.docs.map(doc => doc.data())));
                }
            ).catch(
                err => res.status(500).send(err)
            );
    }
);

appLog.post(
    '/:id',
    (req, res) => {
        const animalId = req.params.id;
        const {lat, lng} = req.body.coordinate;

        return firestore.collection('log')
            .add(Object.assign({animalId}, req.body))
            .then(
                () => geoAnimal.doc(animalId).update({coordinates: new admin.firestore.GeoPoint(lat, lng)})
            ).then(
                () => res.status(200).end()
            ).catch((e) => res.status(500).send(e));
    }
);

appAnimal.get(
    '/:id',
    (req, res) => {
        const animalId = req.params.id;
        return geoAnimal.doc(animalId).get()
        // return firestore.collection('animal').doc(animalId).get()
            .then(
                doc => {
                    if (!doc.exists) {
                        res.status(204).end();
                        return;
                    }
                    const animal = doc.data();
                    animal.coordinate = {lat: animal.coordinates.latitude, lng: animal.coordinates.longitude}
                    return res.status(200).end(JSON.stringify(animal));
                }
            ).catch(err => res.status(500).send(err));
    }
);

appAnimal.get(
    '/',
    (req, res) => {
        return geoAnimal.get()
        // return firestore.collection('animal').get()
            .then(
                snapshot => {
                    if (snapshot.empty) {
                        res.status(204).end();
                        return;
                    }

                    return res.status(200).end(
                        JSON.stringify(
                            snapshot.docs.map(doc => {
                                const animal = doc.data();
                                animal.coordinate = {lat: animal.coordinates.latitude, lng: animal.coordinates.longitude}
                                return animal;
                            })
                        )
                    );
                }
            ).catch(
                err => res.status(500).send(err)
            );
    }
);

appAnimal.post(
    '/:id',
    (req, res) => {
        const animalId = req.params.id;
        const {coordinate} = req.body;
        // return firestore.collection('animal').doc(animalId).set(Object.assign({coordinates: coordinate}, req.body)).then(
        return geoAnimal.doc(animalId).set(
            Object.assign({coordinates: new admin.firestore.GeoPoint(coordinate.lat, coordinate.lng)}, req.body)
        ).then(
            () => res.status(200).end()
        ).catch((e) => res.status(500).send(e));
    }
);


exports.getFeed = functions.https.onRequest((request, response) => {
    const feed = [
        {
            "title": "Zimbabwe ships more than 30 baby elephants, torn from the wild, to Chinese zoos",
            "description": "Just recently, we had acquired footage of the baby elephants eating dry branches and walking around a small water hole in an enclosure in Hwange National Park. Photo by Oscar Nkala More than thirty baby elephants, torn from their mothers in the wild in Zimbabwe almost a year ago, embarked on a new journey of captivity and suffering this week when they were flown to China, where they are expected to spend the rest of their&#160;.&#160;.&#160;.&#160;The post Zimbabwe ships more than 30 baby elephants, torn from the wild, to Chinese zoos appeared first on A Humane World.",
            "link": "https://blog.humanesociety.org/2019/10/zimbabwe-ships-more-than-30-baby-elephants-torn-from-the-wild-to-chinese-zoos.html",
            "url": "https://blog.humanesociety.org/2019/10/zimbabwe-ships-more-than-30-baby-elephants-torn-from-the-wild-to-chinese-zoos.html",
            "guid": {
                "_": "https://blog.humanesociety.org/?p=10922",
                "isPermaLink": [
                    "false"
                ]
            },
            "category": "Humane Society International",
            "dc_creator": "Blog Editor",
            "pubDate": "Fri, 25 Oct 2019 19:49:01 +0000",
            "created": 1572032941000,
            "enclosures": [
                {
                    "url": "https://blog.humanesociety.org/wp-content/uploads/2019/10/zimbabwe-baby-elephants-300x187.jpg",
                    "length": "41315",
                    "type": "image/jpg"
                }
            ]
        },
        {
            "title": "BREAKING NEWS: NIH reneges on promise, will not send 44 research chimpanzees to sanctuary",
            "description": "There is no clear evidence that the welfare of the chimpanzees was actually considered in making this decision. Photo by the HSUS By Kitty Block and Sara Amundson In a stunning about-face on its own promise, the National Institutes of Health today announced it will not send 44 chimpanzees, now held by the Alamogordo primate laboratory in New Mexico, to sanctuary. Just last October, NIH Director Francis&#160;.&#160;.&#160;.&#160; The post BREAKING NEWS: NIH reneges on promise, will not send 44 research chimpanzees to sanctuary appeared first on A Humane World.",
            "link": "https://blog.humanesociety.org/2019/10/breaking-news-nih-reneges-on-promise-will-not-send-44-research-chimpanzees-to-sanctuary.html",
            "url": "https://blog.humanesociety.org/2019/10/breaking-news-nih-reneges-on-promise-will-not-send-44-research-chimpanzees-to-sanctuary.html",
            "guid": {
                "_": "https://blog.humanesociety.org/?p=10920",
                "isPermaLink": [
                    "false"
                ]
            },
            "category": "Animal Research and Testing",
            "dc_creator": "Blog Editor",
            "pubDate": "Thu, 24 Oct 2019 18:22:15 +0000",
            "created": 1571941335000,
            "enclosures": [
                {
                    "url": "https://blog.humanesociety.org/wp-content/uploads/2019/10/chimp5_106269-267x200.jpg",
                    "length": "35634",
                    "type": "image/jpg"
                }
            ]
        },
        {
            "title": "ProTECT Act introduced in Congress to ban trophy hunting horror show",
            "description": "A 2017 nationwide poll showed that 69 percent of American voters oppose trophy hunting altogether. Voters also oppose allowing American trophy hunters to bring home the bodies or parts of the elephants and lions they kill abroad by a margin of more than five to one. Photo by Johan Swanepoel/Alamy Stock Photo By Kitty Block and Sara Amundson President Trump has called trophy hunting a “horror show,” but on his watch, the Department of the Interior has dismantled regulations to protect wildlife and made it easier to import trophies of endangered and threatened animals. We have been&#160;.&#160;.&#160;.&#160; The post ProTECT Act introduced in Congress to ban trophy hunting horror show appeared first on A Humane World.",
            "link": "https://blog.humanesociety.org/2019/10/protect-act-introduced-in-congress-to-ban-trophy-hunting-horror-show.html",
            "url": "https://blog.humanesociety.org/2019/10/protect-act-introduced-in-congress-to-ban-trophy-hunting-horror-show.html",
            "guid": {
                "_": "https://blog.humanesociety.org/?p=10917",
                "isPermaLink": [
                    "false"
                ]
            },
            "category": "Humane Society International",
            "dc_creator": "Blog Editor",
            "pubDate": "Wed, 23 Oct 2019 17:43:57 +0000",
            "created": 1571852637000,
            "enclosures": [
                {
                    "url": "https://blog.humanesociety.org/wp-content/uploads/2019/10/elephants_alamyCWFDPW_263125_263125-1-300x194.jpg",
                    "length": "19132",
                    "type": "image/jpg"
                }
            ]
        },
        {
            "title": "BREAKING NEWS: U.S. House passes the PACT Act, cracking down on extreme animal cruelty",
            "description": "The PACT Act builds on the federal animal crush video law that was enacted in 2010 and banned the creation, sale and distribution of obscene videos that show live animals being crushed, burned, drowned, suffocated, impaled or subjected to other forms of heinous cruelty. Photo by Denis Zbukarev/iStock.com By Kitty Block and Sara Amundson The U.S. House has just voted overwhelmingly to crack down on some of the worst and most malicious acts of animal cruelty, including crushing, burning, drowning, suffocating and impaling live animals and sexually exploiting them. The watershed vote takes&#160;.&#160;.&#160;.&#160 The post BREAKING NEWS: U.S. House passes the PACT Act, cracking down on extreme animal cruelty appeared first on A Humane World.",
            "link": "https://blog.humanesociety.org/2019/10/breaking-news-u-s-house-passes-the-pact-act-cracking-down-on-extreme-animal-cruelty.html",
            "url": "https://blog.humanesociety.org/2019/10/breaking-news-u-s-house-passes-the-pact-act-cracking-down-on-extreme-animal-cruelty.html",
            "guid": {
                "_": "https://blog.humanesociety.org/?p=10915",
                "isPermaLink": [
                    "false"
                ]
            },
            "category": "Companion Animals",
            "dc_creator": "Blog Editor",
            "pubDate": "Tue, 22 Oct 2019 21:01:30 +0000",
            "created": 1571778090000,
            "enclosures": [
                {
                    "url": "https://blog.humanesociety.org/wp-content/uploads/2019/10/cat_dog_iStock_000030858202_259979-300x200.jpg",
                    "length": "36922",
                    "type": "image/jpg"
                }
            ]
        },
        {
            "title": "BREAKING NEWS: Macy’s and Bloomingdale’s ditch fur",
            "description": "The Humane Society of the United States is proud to have worked closely with Macy’s behind the scenes in helping the company make this humane-minded decision. Photo by Schaef1/iStock.com In a landmark announcement that brings us closer than ever to the demise of fur, Macy’s Inc. — the parent company of the iconic American department stores Macy’s and Bloomingdale’s — says it will go fur-free by the end of 2020. This includes permanently closing!!! The post BREAKING NEWS: Macy’s and Bloomingdale's ditch fur appeared first on A Humane World.",
            "link": "https://blog.humanesociety.org/2019/10/breaking-news-macys-and-bloomingdales-ditch-fur.html",
            "url": "https://blog.humanesociety.org/2019/10/breaking-news-macys-and-bloomingdales-ditch-fur.html",
            "guid": {
                "_": "https://blog.humanesociety.org/?p=10913",
                "isPermaLink": [
                    "false"
                ]
            },
            "category": "Humane Economy",
            "dc_creator": "Blog Editor",
            "pubDate": "Mon, 21 Oct 2019 20:39:45 +0000",
            "created": 1571690385000,
            "enclosures": [
                {
                    "url": "https://blog.humanesociety.org/wp-content/uploads/2019/10/fox-iStock-1064158754_493664-e1571678403187-300x200.jpg",
                    "length": "23371",
                    "type": "image/jpg"
                }
            ]
        }
    ];

    response.contentType('application/json');
    response.send(feed);

});

exports.getProximityAnimalAlert = functions.https.onRequest(
    (request, response) => {
        const {lat, lng, r} = request.body;

        if (!lat || !lng || !r) {
            return response.status(500).send('No params provided');
        }
        return geoAnimal.near(
            {
                center: new admin.firestore.GeoPoint(lat, lng), radius: r
            }
        ).get().then(
            (snapshot) => {
                if (snapshot.empty) {
                    response.status(204).end();
                    return;
                }

                return response.status(200).end(JSON.stringify(snapshot.docs.map(doc => doc.data())));
            }
        ).catch(
            err => response.status(500).send(err)
        );
    }
);

exports.log = functions.https.onRequest(appLog);

exports.animal = functions.https.onRequest(appAnimal);
