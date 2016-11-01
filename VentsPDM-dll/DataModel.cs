 
namespace VentsPDM_dll
{
    public class DataModel
    {
        /// <summary>
        /// File id in pdm.
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// File name in pdm.
        /// </summary>
        public virtual string FileName { get; set; }
         
        public virtual int FolderId { get; set; } 

        public virtual string Path { get; set; }

        public override string ToString()
        {
            return base.ToString() + "[\nId: " + Id + "\nFile name: " + FileName+ "\nPath: " + Path + "\nFolder id: " + FolderId +"\n]";
        }
    }
}
 
