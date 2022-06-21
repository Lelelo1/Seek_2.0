
const AHRS = require("ahrs");;

exports.value = new AHRS({ sampleInterval: 20, algorithm: 'Madgwick', beta: 0.4 });

