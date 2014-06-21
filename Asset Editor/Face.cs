using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asset_Editor
{
    class Face
    {
        /*private ShapeObject head;
        private ShapeObject leftEye;
        private ShapeObject rightEye;
        private ShapeObject nose;
        private ShapeObject mouth;
        private ShapeObject hair;*/

        //parameters
        public int eyeHeight = 65;
        public int eyeDist = 8;
        public float eyeSize = 1.0f;
        public int noseHeight = 55;
        public float noseSize = 1.0f;
        public int mouthHeight;
        public float mouthSize = 1.0f;
        public int headWidth = 400;
        public int headHeight = 430;
        
        //shapes
        public int eyeType;
        public int noseType;
        public int mouthType;
        public int headType;
        public int hairType;

        public Face()
        {
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
            eyeDist = 8;
            eyeSize = 1.0f;
            noseHeight = 55;
            noseSize = 1.0f;
            mouthHeight = -55;
            mouthSize = 1.0f;
            headWidth = 400;
            headHeight = 430;
        }
    }
}
