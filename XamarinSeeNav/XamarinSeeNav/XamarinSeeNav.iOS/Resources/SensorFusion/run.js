

let ahrsInstance;
function construct(AHRS) {
    ahrsInstance = new AHRS({ sampleInterval: 20, algorithm: 'Madgwick', beta: 0.4, kp: 0.5, ki: 0, doInitialisation: false });
}

function update(gX, gY, gZ, aX, aY, aZ, mX, mY, mZ, intervalSeconds) {
    // let ahrs = new AHRS({ sampleInterval: 20, algorithm: 'Madgwick', beta: 0.4 });
    // ahrs.update(gX, gY, gZ, aX, aY, aZ, mX, mY, mZ);
    // ahrs.getEulerAngles()
    ahrsInstance.update(gX, gY, gZ, aX, aY, aZ, mX, mY, mZ); // intervalSeconds
    return ahrsInstance.getEulerAngles();
}

