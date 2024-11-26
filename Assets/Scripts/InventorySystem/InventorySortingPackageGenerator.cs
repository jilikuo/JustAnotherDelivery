using DragDrop;
using UnityEngine;

public class InventorySortingPackageGenerator : MonoBehaviour, IDragDropGenerator
{
    [Header("Required Fields")]
    [SerializeField] private RandomAddressGenerator packageAddressGen;
    [SerializeField] private RandomStorylineGenerator storylineGenerator;
    [SerializeField] private float imageScale = 75;
    [SerializeField] private float storylineChance = 0.5f;
    [Header("Derived Fields")]
    [SerializeField] private RandomGameObjectGenerator packageIconGen;


    private Inventory inventory;

    private void Start()
    {
        var inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Failed to locate Inventory");
        }
        packageIconGen = GameManager.instance.packageIconGen;
        if (packageIconGen == null)
        {
            Debug.LogError("Failed to locate packageIconGen");
        }
        if (packageAddressGen == null)
        {
            Debug.LogError("Failed to locate packageAddressGen");
        }

        packageAddressGen.Clear();
        foreach (var character in GameManager.instance.npcCollection.NPCList)
        {
            packageAddressGen.AddEntry(new Address(character));
        }

        storylineGenerator.Clear();
        storylineGenerator.Populate();
    }

    private int CalcCost(GameObject icon)
    {
        // Arbitrary cost value, currently based on icon size
        var rect = icon.GetComponent<RectTransform>();
        float cost = Mathf.Max(1, rect.sizeDelta.x * rect.sizeDelta.y);
        return (int)(GameManager.instance.packageValueMultiplier * cost);
    }

    public DragDropObject CreateDragDrop(GameObject parent)
    {
        if (!storylineGenerator.hasEntries())
        {
            return CreateRandomDragDrop(parent);
        }

        return Random.value < storylineChance ? CreateStorylineDragDrop(parent) : CreateRandomDragDrop(parent);
    }

    private DragDropObject CreateStorylineDragDrop(GameObject parent)
    {
        Storyline story = storylineGenerator.GetEntry();
        storylineGenerator.RemoveEntry(story);

        Address address = new Address(story.GetCurrentChapter().GetRecipient());
        GameObject packageIcon = story.GetCurrentChapter().GetPackage();
       
        return InstantiateDragDropObject(parent, packageIcon, address, story.GetID());
    }

    private DragDropObject CreateRandomDragDrop(GameObject parent)
    {
        Address address = packageAddressGen.GetEntry();
        if (address == null)
        {
            return null;
        }
        packageAddressGen.RemoveEntry(address);

        GameObject packageIcon = packageIconGen.GetEntry();

        return InstantiateDragDropObject(parent, packageIcon, address, StorylineID.RandomStorylines);
    }

    private DragDropObject InstantiateDragDropObject(GameObject parent, GameObject packageIcon, Address address, StorylineID storylineID)
    {
        var icon = Instantiate(packageIcon, parent.transform);
        var rect = icon.GetComponent<RectTransform>();
        rect.localScale = new Vector3(imageScale, imageScale, 1f);
        var centerPoint = new Vector2(0.5f, 0.5f);
        rect.pivot = centerPoint;
        rect.anchorMin = centerPoint; // Bottom-left corner
        rect.anchorMax = centerPoint; // Top-right corner      
        icon.transform.position = parent.transform.position;

        var dragDrop = icon.AddComponent<InventorySortingPackage>();
        dragDrop.data = new Package(packageIcon.name, address, storylineID, CalcCost(icon));
        return dragDrop;
    }

    public void ReturnDragDrop(DragDropObject item)
    {
        var package = item.GetComponent<InventorySortingPackage>();
        if (package == null)
        {
            Debug.LogError("Failed to locate Package component");
            return;
        }
        if (package.data == null)
        {
            Debug.LogError("Failed to locate Package data");
            return;
        }

        if (package.data.storylineID == StorylineID.RandomStorylines)
        {
            packageAddressGen.AddEntry(package.data.address);
        }
        else
        {
            Storyline story = StorylineManager.instance.GetStorylineByID(package.data.storylineID);
            storylineGenerator.AddEntry(story);
        }
        
        Destroy(item.gameObject);
    }
}
