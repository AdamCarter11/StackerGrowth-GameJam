using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] Transform towerTransform;
    [SerializeField] float placementThreshold = 0.1f;
    [SerializeField] LayerMask towerLayerMask; // Set this to the layer of your tower objects

    float towerTop;
    private bool isPlacingBlock = false;
    private GameObject currentBlock;
    Transform lastCubePos;
    float currentBlockXScale;
    GameObject lastObjHit;

    private void Start()
    {

        currentBlockXScale = blockPrefab.transform.localScale.x;
        towerTop = blockPrefab.transform.localScale.y;
        lastObjHit = towerTransform.gameObject;
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space) && !isPlacingBlock)
        {
            // Spawn a new block and make it a child of the tower
            currentBlock = Instantiate(blockPrefab, transform.position, Quaternion.identity);
            currentBlock.transform.parent = towerTransform; // Set to your tower's transform

            isPlacingBlock = true;
        }
        */
        //if (isPlacingBlock)
        //{
            // Move the block horizontally with player input

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPlacingBlock = false;

                // Check if the block is balanced on the tower
                CheckBalancedPlacement();
            }
        //}
    }

    void CheckBalancedPlacement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, towerLayerMask);
        //print(hit.transform.name);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject == lastObjHit)
            {
                //print(hit.transform.name + " hit x pos: " + Mathf.Abs(transform.position.x - hit.point.x));
                float distance = Mathf.Abs(transform.position.x - hit.transform.position.x);
                //print("Distance: " + distance);

                currentBlock = Instantiate(blockPrefab, new Vector3(transform.position.x , towerTransform.position.y + towerTop, transform.position.z), Quaternion.identity);
                currentBlock.transform.localScale = new Vector3(currentBlockXScale, currentBlock.transform.localScale.y, currentBlock.transform.localScale.z);
                
                //currentBlock.transform.parent = towerTransform; // Set to your tower's transform
                towerTop += blockPrefab.transform.localScale.y;

                if (distance < placementThreshold)
                {
                    // The block is balanced on the tower
                    // Allow the tower to grow and increase the score
                    // You can also play a success sound or show a visual effect
                    print("grow tower");
                    currentBlock.transform.position = new Vector2(lastObjHit.transform.position.x, currentBlock.transform.position.y);
                    lastObjHit = currentBlock;

                }
                else
                {
                    // The block is off-balance, remove the overhanging parts
                    // You might want to play a fail sound or show a visual effect
                    RemoveOffBalanceParts(hit.collider.transform);
                    lastObjHit = currentBlock;
                }
            }
            else
            {
                print("Missed top!");
            }
        }
    }

    void RemoveOffBalanceParts(Transform towerTop)
    {
        // Calculate the overhanging part of the block
        //print("top pos: " + towerTop.transform.position.x);
        float overhang = currentBlock.transform.position.x - towerTop.position.x;
        //print("overhang:" + overhang);

        if(overhang > currentBlock.transform.localScale.x)
        {
            //Destroy(currentBlock);
            print("didn't fit");
        }
        else
        {
            // Remove the overhanging parts by resizing the block
            // Example: currentBlock.transform.localScale = new Vector3(overhang * 2, currentBlock.transform.localScale.y, currentBlock.transform.localScale.z);
            currentBlockXScale = currentBlock.transform.localScale.x - Mathf.Abs(overhang);
            currentBlock.transform.localScale = new Vector3(currentBlockXScale, currentBlock.transform.localScale.y, currentBlock.transform.localScale.z);
            transform.localScale = new Vector3(currentBlockXScale, currentBlock.transform.localScale.y, currentBlock.transform.localScale.z);
            //print("curr x: " + currentBlock.transform.position.x + "overhang: " + overhang);
            //currentBlock.transform.position = new Vector2(Mathf.Round((transform.position.x - overhang) * 100.0f) / 100.0f , currentBlock.transform.position.y);
            currentBlock.transform.position = new Vector2(lastObjHit.transform.position.x, currentBlock.transform.position.y);
            print("curr block pos: " + currentBlock.transform.position.x);
        }

        // You might want to play a falling sound or show a visual effect
        // Then, destroy the block and handle the game over scenario if needed
        //Destroy(currentBlock);
    }
}

