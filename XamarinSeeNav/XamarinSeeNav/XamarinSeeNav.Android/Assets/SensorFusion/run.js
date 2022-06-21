
function update(logarithm, gX, gY, gZ, aX, aY, aZ, mX, mY, mZ) {
    logarithm.update(gX, gY, gZ, aX, aY, aZ, mX, mY, mZ);
    return madgwick.getEulerAngles();
}
