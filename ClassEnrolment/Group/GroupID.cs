namespace StudentsGroup
{
    internal class GroupID
    {
        private string _id;

        public GroupID(string id)
        {
            this._id = id;
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        static public bool operator ==(GroupID first, GroupID second)
        {
            if (first.Equals(second))
                return true;

            return (object)first != null && (object)second != null && first._id == second._id;
        }

        static public bool operator !=(GroupID first, GroupID second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}