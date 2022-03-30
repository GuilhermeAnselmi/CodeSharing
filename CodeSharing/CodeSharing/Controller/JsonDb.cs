using CodeSharing.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeSharing.Controller
{
    internal class JsonDb
    {
        private string FilePath { get; set; }
        private string FileName { get; set; }

        public JsonDb()
        {
            FileName = "Coding.json";
            FilePath = Path.GetTempPath() + FileName;
        }

        public Tuple<bool, string> CreateBlock(string blockName)
        {
            //MyBlocks mbs = new MyBlocks();
            List<MyBlock> mbs = new List<MyBlock>();
            Tuple<bool, string> response = null;

            try
            {
                if (File.Exists(FilePath))
                {
                    mbs = JsonConvert.DeserializeObject<List<MyBlock>>(File.ReadAllText(FilePath));

                    response = FindDuplicate(blockName);

                    if (!response.Item1) return response;
                }
                else
                {
                    var file = File.Create(FilePath);
                    file.Close();
                }
            
                List<MyBlock> blocks = new List<MyBlock>();
                if (mbs != null) blocks = mbs;
                blocks.Add(new MyBlock() { BlockName = blockName, CodeList = "" });

                mbs = blocks;

                string json = JsonConvert.SerializeObject(mbs);

                File.WriteAllText(FilePath, json);

                response = new Tuple<bool, string>(true, "New block created succesafully");
            }
            catch (Exception ex)
            {
                response = new Tuple<bool, string>(false, "Create block this error: " + ex);
            }

            return response;
        }

        public Tuple<bool, string> FindDuplicate(string blockName)
        {
            Tuple<bool, string> response;

            if (File.Exists(FilePath))
            {
                List<MyBlock> mbs = new List<MyBlock>();

                mbs = JsonConvert.DeserializeObject<List<MyBlock>>(File.ReadAllText(FilePath));

                var find = mbs.Where(x => x.BlockName == blockName).ToList();

                if (find.Count == 0)
                {
                    response = new Tuple<bool, string>(true, null);
                }
                else
                {
                    response = new Tuple<bool, string>(false, "Block Name Already Exists");
                }
            }
            else
            {
                response = new Tuple<bool, string>(false, "File Not Found: Error (MyCode.FindDuplicate)");
            }

            return response;
        }

        public MyBlock[] ListAllBlocks()
        {
            MyBlock[] block = null;

            try
            {
                if (File.Exists(FilePath))
                {
                    block = JsonConvert.DeserializeObject<MyBlock[]>(File.ReadAllText(FilePath));
                }

                return block;
            }
            catch
            {
                return block;
            }
        }

        public Tuple<bool, string> RewriteBlock(MyBlock[] blocks, string newName)
        {
            Tuple<bool, string> response = new Tuple<bool, string>(false, "Error: Generic");

            try
            {
                response = FindDuplicate(newName);

                if (response.Item1)
                {
                    if (File.Exists(FilePath))
                    {
                        File.WriteAllText(FilePath, JsonConvert.SerializeObject(blocks));

                        response = new Tuple<bool, string>(true, "");
                    }
                    else
                    {
                        response = new Tuple<bool, string>(false, "File not found");
                    }
                }
            }
            catch (Exception ex)
            {
                response = new Tuple<bool, string>(false, "Error: " + ex);
            }

            return response;
        }

        public Tuple<bool, string> RemoveBlock(string blockName)
        {
            Tuple<bool, string> response = new Tuple<bool, string>(false, "File Not Found");

            try
            {
                if (File.Exists(FilePath))
                {
                    MyBlock[] blocks = JsonConvert.DeserializeObject<MyBlock[]>(File.ReadAllText(FilePath));

                    List<MyBlock> newBlocks = blocks.Where(x => x.BlockName != blockName).ToList();

                    blocks = newBlocks.ToArray();

                    File.WriteAllText(FilePath, JsonConvert.SerializeObject(blocks));

                    response = new Tuple<bool, string>(true, "");
                }
                else
                {
                    response = new Tuple<bool, string>(false, "File not found");
                }
            }
            catch(Exception ex)
            {
                response = new Tuple<bool, string>(false, "Error: " + ex);
            }

            return response;
        }

        public Tuple<bool, string> CodeChanges(string blockName, string code)
        {
            Tuple<bool, string> response = new Tuple<bool, string>(false, "File Not Found");

            try
            {
                if (File.Exists(FilePath))
                {
                    List<MyBlock> mbs = new List<MyBlock>();

                    mbs = JsonConvert.DeserializeObject<List<MyBlock>>(File.ReadAllText(FilePath));

                    mbs.Where<MyBlock>(x => x.BlockName == blockName).First().CodeList = code;

                    File.WriteAllText(FilePath, JsonConvert.SerializeObject(mbs));

                    response = new Tuple<bool, string>(true, "");
                }
            }
            catch (Exception ex)
            {
                response = new Tuple<bool, string>(false, "Error: " + ex);
            }

            return response;
        }
    }
}
