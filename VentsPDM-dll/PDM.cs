using System;
using EdmLib;
using System.Collections.Generic;
using System.IO;

namespace VentsPDM_dll
{
    public class PDM  
    {

        public PDM()
        {
            PDMInitialize();
        }
        /// <summary>
        /// PDM exemplar.
        /// </summary>
        private IEdmVault5 edmVault5 = null;

        /// <summary>
        /// Vents pdm name.
        /// </summary>
        public string vaultname { get; set; } = "Vents-PDM";


        /// <summary>
        /// Search document by name.
        /// </summary>
        /// <param name="segmentName"></param>
        /// <returns></returns>
        public IEnumerable<DataModel> SearchDoc(string segmentName)
        {
            this.PDMInitialize();

            
            List<DataModel> searchResult = new List<DataModel>();
            try
            {
                var Search = (edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_Search);
                Search.FileName = "%" + segmentName + "%";
                int count = 0;
                IEdmSearchResult5 Result = Search.GetFirstResult();
                while (Result != null)
                { 
                    

                        searchResult.Add(new DataModel
                        {
                            FileName = Result.Name,
                            Id = Result.ID, 
                            FolderId = Result.ParentFolderID,
                            Path = Result.Path
                        });
                 
                    Result = Search.GetNextResult();
                    count++;
                }
                Logger.ToLog("По запросу " + segmentName + " найдено " + count);
                Logger.ToLog("\n");
            }
            catch (Exception ex)
            { 
                Logger.ToLog("По запросу " + segmentName + " не найдено ни одного файла\n Ошибка: " + ex);              
               
            }
            return searchResult;
        }


        /// <summary>
        /// Download file in to local directory witch has fixed path
        /// </summary>
        /// <param name="dataModel"></param>
        public void DownLoadFile(DataModel dataModel)
        {
            this.PDMInitialize();
            try
            {
                var batchGetter = (IEdmBatchGet)(edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_BatchGet);
                batchGetter.AddSelectionEx((EdmVault5)edmVault5, dataModel.Id, dataModel.FolderId, 0);
                if ((batchGetter != null))
                {
                    batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_SkipUnlockedWritable);
                    batchGetter.GetFiles(0, null);
                }
                Logger.ToLog("Файл " + dataModel.FileName + " с id " + dataModel.Id + " успешно скачан с PDM системы по пути " + dataModel.Path);
            }
            catch (Exception ex)
            {
                Logger.ToLog("Ошибка при скачивании файла " + dataModel.FileName + " с id " + dataModel.Id + ex.ToString());

            }
        }
        

        public string CloneDowladFileTo (string directoryPath, DataModel dataModel)
        {
            string[] splitFileName = dataModel.FileName.Split('.');
            string fileExtension = splitFileName[splitFileName.Length - 1];          
            directoryPath = directoryPath + dataModel.Id+"." + fileExtension;
            
            try
            { 
                    if (Directory.Exists(directoryPath))
                    {
                        Logger.ToLog("Файл " + directoryPath + " уже существует.");
                        File.Delete(directoryPath);
                        Logger.ToLog("Файл " + directoryPath + "удален.");                    
                    }
                    File.Copy(dataModel.Path, directoryPath, true);
                    Logger.ToLog("Файл " + dataModel.FileName + " с id " + dataModel.Id + " успешно скопирован.");
             
            }
            catch (IOException ex)
            {
                Logger.ToLog("Неудалось скопировать файл по имени " + dataModel.FileName + " с Id " + dataModel.Id + "в директорию " + directoryPath + 
                    "\n по причине обозначеной в исключении " + ex); 
            }

            return directoryPath;
        }

 

        ///// <summary>
        ///// Download file in to local directory.
        ///// </summary>
        ///// <param name="dataModel"></param>
        //public string GetPathAndDownLoadFile(DataModel dataModel, string directoryPath)
        //{          
        //    this.PDMInitialize(); 
        //    IEdmFolder5 srcFolder = null;
        //    IEdmFile5 file = edmVault5.GetFileFromPath(dataModel.Path,out srcFolder);
        //    IEdmVault7 vault2 = (IEdmVault7)this.edmVault5;
        //    IEdmFolder5 edmFolder5 = vault2.GetFolderFromPath(directoryPath);
        //    IEdmFolder8 edmFolder8 = (IEdmFolder8)edmFolder5;

        //    int copyFileStatus;
        //    edmFolder8.CopyFile2(file.ID, srcFolder.ID, 0, out copyFileStatus, "", (int)EdmCopyFlag.EdmCpy_Simple);

        //    //var batchGetter = (IEdmBatchGet)(edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_BatchGet);
        //    // batchGetter.AddSelectionEx((EdmVault5)edmVault5, dataModel.Id, dataModel.FolderId, 0);
        //    // if ((batchGetter != null))
        //    // {
        //    //     batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_SkipUnlockedWritable);
        //    //     batchGetter.GetFiles(0, null);
        //    // }
        //    return null;
        //}

        /// <summary>
        /// Reference in to the components of assembly.  
        /// </summary>
        /// <param name="file"></param>
        /// <param name="indent"></param>
        /// <param name="projName"></param>
        /// <returns></returns>
        private string AddReferences(IEdmReference5 file, long indent, ref string projName)
        {
            string filename = null;

            filename = filename + file.Name;

            const bool isTop = false;

            IEdmVault7 vault2 = null;
            if (edmVault5 == null)
            {
                edmVault5 = new EdmVault5();
            }
            vault2 = (IEdmVault7)edmVault5;

            if (!edmVault5.IsLoggedIn)
            {
                edmVault5.LoginAuto(vaultname, 0);
            }

            IEdmPos5 pos = file.GetFirstChildPosition(projName, isTop, true, 0);

            IEdmFolder5 oFolder = null;


            while (!(pos.IsNull))
            {
                IEdmReference5 @ref = file.GetNextChild(pos);
                var oFile = (IEdmFile5)edmVault5.GetFileFromPath(@ref.FoundPath, out oFolder);

                filename = filename + AddReferences(@ref, indent, ref projName);

                //MessageBox.Show(filename);
                // Последняя копия перечня в сборке
                oFile.GetFileCopy(0, "", @ref.FoundPath);
            }
            return filename;
        }
        public void ShowReferences(EdmVault5 vault, string filePath)
        {
            // ERROR: Not supported in C#: OnErrorStatement
            string projName = null;
            IEdmFile5 file = default(IEdmFile5);
            IEdmFolder5 folder = default(IEdmFolder5);
            file = vault.GetFileFromPath(filePath, out folder);

            IEdmReference5 @ref = default(IEdmReference5);
            @ref = file.GetReferenceTree(folder.ID, 0);
            AddReferences(@ref, 0, ref projName);


            
        }

        /// <summary>
        /// Pdm initializes an instance of this object by creating and producing auto-login.
        /// </summary>
        private void PDMInitialize()
        {
            try
            {
                if (edmVault5 == null)
                {
                    edmVault5 = new EdmVault5();
                    Logger.ToLog("Создан экземпляр Vents-PDM");
                }

                if (!edmVault5.IsLoggedIn)
                {
                    edmVault5.LoginAuto(vaultname, 0);
                    Logger.ToLog("Автологин в системе Vents-PDM системного пользователя " + vaultname );
                }
            }
            catch
            {
                Logger.ToLog("Невозможно создать экземпляр Vents-PDM - " + this.vaultname);
                throw new Exception("Невозможно создать экземпляр " + this.vaultname);
            }
            finally
            {
                Logger.ToLog("\n");
            }

        }

        ~PDM()  // destructor
        {

        }

    }
}
