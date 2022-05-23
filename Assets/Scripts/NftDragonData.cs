using System;
using System.Collections;
using System.Collections.Generic;

 public class NftDragonData
    {
        public int _id;
        public int _bloodType;
        public String _rarity;
        public String image;

        public String classType;

        public override bool Equals(object obj)
        {
            return obj is NftDragonData data && 
                   _id == data._id &&
                   _bloodType == data._bloodType &&
                   _rarity == data._rarity &&
                   image == data.image &&
                   classType == data.classType;
        }

        public override int GetHashCode()
        {
            int hashCode = 550942935;
            hashCode = hashCode * -1521134295 + _id.GetHashCode();
            hashCode = hashCode * -1521134295 + _bloodType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_rarity);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(image);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(classType);
            return hashCode;
        }
    }
