using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Asset_Editor
{
    [Serializable()]
    class Face
    {
        //parameters
        public int eyeHeight = 65;      //40..90
        public int eyeDist = 10;        //0..20
        public float eyeSize = 0.9f;    //0.65..1.15
        public int noseHeight = 35;     //20..50
        public float noseSize = 0.9f;   //0.65..1.15
        public int mouthHeight = -55;   //-70..-40
        public float mouthSize = 0.9f;  //0.65..1.15
        public int headWidth = 400;     //350..450
        public int headHeight = 430;    //380..480
        
        //shapes
        public int eyeType;             //0..2
        public int noseType;            //0..2
        public int mouthType;           //0..2
        public int headType;            //0..2
        public int hairType;            //0..2

        public Face()
        {
        }

        public Face(SerializationInfo info, StreamingContext ctxt)
        {
            eyeType = (int)info.GetValue("eType", typeof(int));
            noseType = (int)info.GetValue("nType", typeof(int));
            mouthType = (int)info.GetValue("mType", typeof(int));
            headType = (int)info.GetValue("heType", typeof(int));
            hairType = (int)info.GetValue("haType", typeof(int));

            eyeHeight = (int)info.GetValue("eHeight", typeof(int));
            eyeDist = (int)info.GetValue("eDist", typeof(int));
            noseHeight = (int)info.GetValue("nHeight", typeof(int));
            mouthHeight = (int)info.GetValue("mHeight", typeof(int));

            eyeSize = (float)info.GetValue("eSize", typeof(float));
            noseSize = (float)info.GetValue("nSize", typeof(float));
            mouthSize = (float)info.GetValue("mSize", typeof(float));

            headWidth = (int)info.GetValue("hWidth", typeof(int));
            headHeight = (int)info.GetValue("hHeight", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("eType", eyeType);
            info.AddValue("nType", noseType);
            info.AddValue("mType", mouthType);
            info.AddValue("heType", headType);
            info.AddValue("haType", hairType);

            info.AddValue("eHeight", eyeHeight);
            info.AddValue("eDist", eyeDist);
            info.AddValue("nHeight", noseHeight);
            info.AddValue("mHeight", mouthHeight);

            info.AddValue("eSize", eyeSize);
            info.AddValue("nSize", noseSize);
            info.AddValue("mSize", mouthSize);

            info.AddValue("hWidth", headWidth);
            info.AddValue("hHeight", headHeight);
        }

        public void init()
        {
            Random ran = new Random();
            eyeType = ran.Next(0,3);
            noseType = ran.Next(0,3);
            mouthType = ran.Next(0, 3);
            headType = ran.Next(0, 3);
            hairType = ran.Next(0, 3);
            eyeHeight = 65;
            eyeDist = 10;
            eyeSize = 0.9f;
            noseHeight = 35;
            noseSize = 0.9f;
            mouthHeight = -55;
            mouthSize = 0.9f;
            headWidth = 400;
            headHeight = 430;
        }
    }
}
