using CodeSharing.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeSharing.Controller
{
    internal class MyCode
    {
        private string FilePath { get; set; }
        private string FileName { get; set; }
        private string BlockName { get; set; }
        private string Code { get; set; }
        private string Json { get; set; }

        public MyCode(string blockName)
        {
            FileName = "Coding.json";
            FilePath = Path.GetTempPath() + FileName;
            BlockName = blockName;
        }

        public void CreateBlock()
        {
            //MyBlocks mbs = new MyBlocks();
            List<MyBlock> mbs = new List<MyBlock>();

            if (File.Exists(FilePath))
            {
                mbs = JsonConvert.DeserializeObject<List<MyBlock>>(File.ReadAllText(FilePath));

                FindDuplicate();
            }
            else
            {
                var file = File.Create(FilePath);
                file.Close();
            }

            List<MyBlock> blocks = new List<MyBlock>();
            if (mbs != null) blocks = mbs;
            blocks.Add(new MyBlock() { BlockName = BlockName });

            mbs = blocks;

            string json = JsonConvert.SerializeObject(mbs);

            File.WriteAllText(FilePath, json);
        }

        public Tuple<bool, string> FindDuplicate()
        {
            Tuple<bool, string> response;

            if (File.Exists(FilePath))
            {
                List<MyBlock> mbs = new List<MyBlock>();

                mbs = JsonConvert.DeserializeObject<List<MyBlock>>(File.ReadAllText(FilePath));

                var find = mbs.Where(x => x.BlockName == BlockName).ToList();

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
    }
}
