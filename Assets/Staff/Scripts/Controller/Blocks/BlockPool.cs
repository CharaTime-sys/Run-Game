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
}
