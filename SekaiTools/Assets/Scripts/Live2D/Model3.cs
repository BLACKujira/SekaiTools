using System;

namespace SekaiTools.Live2D
{
    [Serializable]
    public class Model3
    {
        public int Version;
        public Filereferences FileReferences;
        public Group[] Groups;

        public Model3(string moc, string[] textures, string physics)
        {
            Version = 3;
            FileReferences = new Filereferences(moc,textures,physics);
            Groups = new Group[2];
            Groups[0] = new Group("Parameter", "EyeBlink");
            Groups[1] = new Group("Parameter", "LipSync");
        }
    }


    [Serializable]
    public class Filereferences
    {
        public string Moc;
        public string[] Textures;
        public string Physics;

        public Filereferences(string moc, string[] textures, string physics)
        {
            Moc = moc;
            Textures = textures;
            Physics = physics;
        }
    }

    [Serializable]
    public class Group
    {
        public string Target;
        public string Name;
        public int[] Ids = new int[0];

        public Group(string target, string name)
        {
            this.Target = target;
            this.Name = name;
        }
    }
}