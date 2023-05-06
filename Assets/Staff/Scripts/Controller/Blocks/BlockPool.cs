using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BlockPool : MonoBehaviour
{
    public Block[] blocks;
    ObjectPool<Block>[] block_pools;
    public int pool_num;
    public int pool_size;
    public int pool_max_size;
    public int index;
    public Material material;
    public Material material_1;
    private void Awake()
    {
        block_pools = new ObjectPool<Block>[pool_num];
        for (int i = 0; i < pool_num; i++)
        {
            index = i;
            block_pools[i] = new ObjectPool<Block>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, true, pool_size);
        }
    }

    Block OnCreatePoolItem()
    {
        var block = Instantiate(blocks[index], transform);
        block.SetDeactivateAction(delegate { block_pools[index].Release(block); });
        return block;
    }

    void OnGetPoolItem(Block block)
    {
        block.gameObject.SetActive(true);
    }

    void OnReleasePoolItem(Block block)
    {
        block.gameObject.SetActive(false);
    }

    void OnDestroyPoolItem(Block block)
    {
        Destroy(block.gameObject);
    }
    [ContextMenu("ÐÞ¸Ä")]
    public void Change_Pos()
    {
        foreach (Transform item in transform)
        {
            if (item.name.Contains("Jump"))
            {
                item.GetComponent<MeshRenderer>().material = material;
                item.GetChild(0).GetComponent<MeshRenderer>().material = material_1;
            }
            if (item.name.Contains("Down") && !item.name.EndsWith("1"))
            {
                item.GetComponent<MeshRenderer>().material = material;
            }
            //if (item.name.Contains("Down2"))
            //{
            //    item.localPosition = new Vector3(item.localPosition.x, -0.68f, item.localPosition.z);
            //}
            //if (item.name.Contains("Monster 1"))
            //{
            //    item.localPosition = new Vector3(item.localPosition.x, -6.19f, item.localPosition.z);
            //}
            //if (item.name.Contains("Monster 2"))
            //{
            //    item.localPosition = new Vector3(item.localPosition.x, -6.55f, item.localPosition.z);
            //}
            //if (item.name.Contains("Buff"))
            //{
            //    item.localPosition = new Vector3(item.localPosition.x, -7.8f, item.localPosition.z);
            //}
        }
    }
}
