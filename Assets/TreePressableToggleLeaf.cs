using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace ARDataVisualization
{
    public class TreePressableToggleLeaf : MonoBehaviour
    {
        public TreeNode TreeNode { get; private set; }
        private TreeVisualization treeVisualization = null;
        [SerializeField] private ToolTipConnector connector;
        [SerializeField] private Interactable interactable;
        
        
        public void InitializeLeaf(TreeNode treeNode, TreeVisualization treeVisualization, GameObject connectTo = null)
        {
            //Keep self tree node and a reference to TreeVisualization.
            TreeNode = treeNode;
            treeVisualization = null;
            
            //Connect to a given leaf.
            if (connectTo != null)
            {
                connector.Target = connectTo;
            }
            else //Deactive spline in case that the leaf is root.
            {
                connector.gameObject.gameObject.SetActive(false);
            }
        }
    }
}
