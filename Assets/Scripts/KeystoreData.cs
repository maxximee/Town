 [System.Serializable]
 public class KeystoreData
 {
     public string fileName;
     public string contentAsJson;
 
     public KeystoreData(string fileNameStr, string contentAsJsonStr)
     {
         fileName = fileNameStr;
         contentAsJson = contentAsJsonStr;
     }
 }