using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> birdPrefabs;
    public List<GameObject> inGameSpawnBirds;

    [Header("Transform")]
    [SerializeField] private Transform spawnPoint;

    [Header("Sript Reference")]
    [SerializeField] SlingShotController slingShotController;
    public TrajectoryLine trajectoryLine;

    [Header("UI Btn")]
    [SerializeField] private Button btn_birdSelect0;
    [SerializeField] private Button btn_birdSelect1;
    [SerializeField] private Button btn_birdSelect2;
    public Button[] birdSelectBtns;

    public int birdCount = 3;
    public bool canShoot = true; // After bird which was shooted before disappeared, Can shoot


    private void Start()
    {
        InitBird();
    }

    public void InitBird()
    {
        // List Clear => only delete reference(except Value data(ex) int, float, char..))
        // so if I wanna destroy the gameobject in hierarchy, I have to directly approach the gameobject 
        foreach(var bird in inGameSpawnBirds) // foreach => approaching the object data referencing the address in the list!!!
        {
            Destroy(bird);
        }

        inGameSpawnBirds.Clear(); // Clear => use the original list, new list<T>() => make new list (By using AddFunc, i can handle the list dynamically)

        for(int i = 0; i < birdPrefabs.Count; i++)
        {
            var birdClone = Instantiate(birdPrefabs[i], spawnPoint.position, Quaternion.identity);
            birdClone.SetActive(false);
            inGameSpawnBirds.Add(birdClone);
        }
    }

    public void InitBtn()
    {
        for (int i = 0; i < birdSelectBtns.Length; i++)
        {
            birdSelectBtns[i].interactable = true;
            birdSelectBtns[i].image.color = Color.green;
        }
    }


    public void OnBtnChangeBird(int index)
    {
        // Changing the bird? I should delete the bird that already exist
        // After shooting bird? => button interactable off, color grey

        if (inGameSpawnBirds[index] != null && canShoot)
        {
            // Turn off all the birds
            for (int i = 0; i < inGameSpawnBirds.Count; i++)
            {
                inGameSpawnBirds[i].SetActive(false);
            }

            // Turn on the bird I selected
            GameObject birdClone = inGameSpawnBirds[index];
            birdClone.SetActive(true);

            BaseBird bird = birdClone.GetComponent<BaseBird>();

            // Send data to Trajectory Script 
            trajectoryLine.SetUp(bird);

            // Send this script, slingShotController script (which's allocated in inspector) to BaseBird script 
            birdClone.GetComponent<BaseBird>().SetUp(slingShotController, this, index);
        }
    }

}
