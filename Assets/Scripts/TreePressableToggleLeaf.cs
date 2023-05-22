using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDataVisualization
{
    public class TreePressableToggleLeaf : MonoBehaviour
    {
        public TreeVisualization.TreeNode TreeNode { get; private set; }
        private TreeVisualization treeVisualization = null;

        [Header("TreeLeaf Components")]
        [SerializeField] private ToolTipConnector connector;
        [SerializeField] private Interactable interactable;
        [SerializeField] private ButtonConfigHelper buttonConfigHelper;
        [SerializeField] private ToolTip tooltip;

        [Header("Positioning Children Settings")]
        [SerializeField] private Vector3 firstChildLeafPosition;
        [SerializeField] private Vector3 offsetChildLeafPosition;

        private List<GameObject> childrenLeafObj = new List<GameObject>();
        
        public void InitializeLeaf(TreeVisualization.TreeNode treeNode, TreeVisualization treeVisualization, GameObject connectTo = null)
        {
            //Keep self tree node and a reference to TreeVisualization.
            TreeNode = treeNode;
            this.treeVisualization = treeVisualization;
            
            //Connect to a given leaf.
            if (connectTo != null)
            {
                connector.Target = connectTo;
                tooltip.ToolTipText = TreeNode.value;
                buttonConfigHelper.MainLabelText = "";
            }
            else //Deactive spline in case that the leaf is root.
            {
                connector.gameObject.gameObject.SetActive(false);
                buttonConfigHelper.MainLabelText = TreeNode.value.ToString();
            }

            //Listen to toggle press
            childrenLeafObj.Clear();
            interactable.OnClick.AddListener(() => { ToggleChanged(interactable.IsToggled); });
            
        }

        private void ToggleChanged(bool isToggled)
        {
            if (isToggled)
            {
                Vector3 place = firstChildLeafPosition;
                foreach(TreeVisualization.TreeNode child in TreeNode.Children)
                {
                    GameObject leafObj = treeVisualization.CreateTreeLeaf(child, place, gameObject);
                    childrenLeafObj.Add(leafObj);
                    place += offsetChildLeafPosition;
                }
            }
            else
            {
                while(childrenLeafObj.Count > 0)
                {
                    GameObject leafObj = childrenLeafObj.Last();
                    childrenLeafObj.Remove(leafObj);
                    Destroy(leafObj);
                }
            }

        }


    }
}
