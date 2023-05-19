using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using ARDataVisualization.Utility;
using System.ComponentModel.Design.Serialization;

namespace ARDataVisualization
{
    public class TreeVisualization : MonoBehaviour
    {
        [SerializeField]
        private string dataFilePath = "";

        private DataTable data;
        private List<TreeNode> dataTrees = new List<TreeNode>();

        // Start is called before the first frame update
        private void Start()
        {
            UpdateData();
            GenerateTrees();
        }

        private void UpdateData()
        {
            data = CSVFileReader.ReadCSVFile(dataFilePath);
        }

        private void GenerateTrees()
        {
            dataTrees.Clear();
            DataRowCollection rows = data.Rows;
            for(int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                DataRow row = rows[rowIndex];
                TreeNode traverseNode = null;
                string rootValue = row.ItemArray[0].ToString();
                traverseNode = dataTrees.Find((node) => node.value == rootValue);
                if(traverseNode == null )
                {
                    traverseNode = new TreeNode(rootValue);
                    dataTrees.Add(traverseNode);
                }

                //Traversing the tree while adding new Tree nodes with row's data.
                foreach (string val in row.ItemArray)
                {
                    if(traverseNode.value == val)
                    {
                        continue;
                    }

                    if(traverseNode.Children.Exists((node) => node.value == val))
                    {
                        traverseNode = traverseNode.Children.Find((node) => node.value == val);
                    }
                    else
                    {
                        traverseNode = traverseNode.AddChild(new TreeNode(val));
                    }
                }
            }

        }

        public class TreeNode
        {
            public string value;
            private List<TreeNode> children;

            public List<TreeNode> Children => new List<TreeNode>(children);

            public TreeNode(string data)
            {
                this.value = data;
                this.children = new List<TreeNode>();
            }

            public TreeNode AddChild(TreeNode node)
            {
                children.Add(node);
                return node;
            }
        }
    }
}
