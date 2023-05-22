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
        [Header("Read Data Source")]
        [SerializeField]
        private string dataFilePath = "";

        [Header("Leaf Prefabs")]
        [SerializeField] private GameObject toggleLeaf;

        [Header("Positioning Settings")]
        [SerializeField] private Vector3 firstTreePosition;
        [SerializeField] private Vector3 offsetTreePosition;


        private DataTable data;
        private List<TreeNode> dataTrees = new List<TreeNode>();
        private List<GameObject> treesObj = new List<GameObject>();



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
            treesObj.Clear();
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

            Vector3 place = firstTreePosition;
            foreach (TreeNode tree in dataTrees)
            {
                GameObject treeObj = CreateTreeLeaf(tree, place, null);
                place += offsetTreePosition;
                treesObj.Add(treeObj);
            }

        }

        public GameObject CreateTreeLeaf(TreeVisualization.TreeNode node, Vector3 instantiatePlace, GameObject fatherLeaf)
        {
            //Right now it creates only toggle leafs
            GameObject leaf = null;
            if(fatherLeaf != null)
            {
                leaf = Instantiate(toggleLeaf, fatherLeaf.transform, false);
                leaf.transform.localPosition = instantiatePlace;
            }
            else
            {
                leaf = Instantiate(toggleLeaf, instantiatePlace, Quaternion.identity);
            }
            TreePressableToggleLeaf pressableToggleLeaf = leaf.GetComponent<TreePressableToggleLeaf>();
            //if(pressableToggleLeaf != null )
            {
                pressableToggleLeaf.InitializeLeaf(node, this, fatherLeaf);
            }

            return leaf;

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
