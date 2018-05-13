using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TextAnimScript : MonoBehaviour {
    
    private string textString;
    private int currentTextCount = 0;
    private int startingTextPosY;

    [SerializeField]
    GameObject textPivot;
    GameObject textBlock;

    [SerializeField]
    int verticalSpacing;

    [SerializeField]
    float animLineSpeed;
    [SerializeField]
    float animBlockSpeed;
    [SerializeField]
    float animBlockDeleteSpeed;
    
    // String lists (paragraphs)
    [SerializeField]
    List<string> textStringList01;
    [SerializeField]
    List<string> textStringList02;
    [SerializeField]
    List<string> textStringList03;
    [SerializeField]
    List<string> textStringList04;
    [SerializeField]
    List<string> textStringList05;


    // Main List that the above lists are copied and read from
    private List<List<string>> textStringLists = new List<List<string>>();

    // Used mainly for storage of the gameObjects in scene, then destroyed after paragraph
    private List<GameObject> textBlockList = new List<GameObject>();

    // Text mesh element, changing variable depending on current block of text created
    private TextMeshPro textMesh;

   
    // Use this for initialization
    private void Awake () {
        
        textStringLists.AddRange(new List<List<string>> { textStringList01, textStringList02,
                                                        textStringList03, textStringList04,
                                                        textStringList05 } );
        
    }

    private void Start() {

        StartCoroutine(AddTextBlock());

    }

    private void CreateTextBlock(string item) {
        
        // Create new textBlock
        textBlock = (GameObject)Instantiate(textPivot, new Vector3(0, startingTextPosY, 0), new Quaternion(0, 0, 0, 0));
        
        // Add it to list (hacky)
        textBlockList.Add(textBlock);

        // Set the text from string list on TextBlockHandler gameObject
        textMesh = textBlock.GetComponentInChildren<TextMeshPro>();
        textMesh.SetText("  " + item);

        //TODO: this may stay redundant, if so remove
        currentTextCount++;

        foreach (GameObject textItem in textBlockList)
        {

            if (textItem == null) return;
            // This moves all textblocks up by "verticalSpacing" 
            textItem.transform.position = new Vector3(textItem.transform.position.x,
                                                        textItem.transform.position.y + verticalSpacing,
                                                        textItem.transform.position.z);
        }
    }


    IEnumerator AddTextBlock() {
        
        // Loop over list of lists
        foreach (List<string> subList in textStringLists)
        {
            
            // Loop over list of strings (one list is one paragraph)
            foreach (string item in subList) {

                // How long delay between lines
                yield return new WaitForSeconds(animLineSpeed);
                CreateTextBlock(item);
            }

            // How long delay between deleting block
            yield return new WaitForSeconds(animBlockDeleteSpeed);
                
            // Clear GameObjects string blocks from scene while list still contains them
            foreach (GameObject textItem in textBlockList) {
                Destroy(textItem);
            }

            //TODO: just clear list rather than make new
            textBlockList = new List<GameObject>();
            
            // How long delay between starting new block
            yield return new WaitForSeconds(animBlockSpeed);
            
        }
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}
