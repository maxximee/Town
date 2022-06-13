using System.Numerics;
using System.Collections;
using System.Collections.Generic;


public class DragonDataModel {
    public BigInteger maxSpeed;
    public BigInteger acceleration;
    public BigInteger yield;
    public BigInteger diet;

    public DragonDataModel(BigInteger maxSpeed, BigInteger acceleration, BigInteger yield, BigInteger diet) {
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.yield = yield;
        this.diet = diet;
    }
}
