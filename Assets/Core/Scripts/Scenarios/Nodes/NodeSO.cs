using UnityEngine;

[CreateAssetMenu(fileName = "NodeSO", menuName = "NodeSO", order = 0)]
public class NodeSO : ScriptableObject
{
    [SerializeField]
    public Node node;
}