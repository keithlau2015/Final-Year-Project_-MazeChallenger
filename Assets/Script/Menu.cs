using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Layer Mask
    private LayerMask clickable;

    private Transform _selection;

    private Camera camera;
    private RaycastHit hit;

    //GameObject
    [SerializeField] private Collider cog, cog_2, cog_3, cog_4, door;

    private Animator cogAnimation, cog_2Animation, cog_3Animation, cog_4Animation, doorAnimator;

    private void Start()
    {
        camera = Camera.main;
        cogAnimation = cog.GetComponent<Animator>();
        cog_2Animation = cog_2.GetComponent<Animator>();
        cog_3Animation = cog_3.GetComponent<Animator>();
        cog_4Animation = cog_4.GetComponent<Animator>();
        doorAnimator = door.GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (_selection != null)
        {
            //Cog
            if (_selection.CompareTag("Setting"))
            {
                cogAnimation.SetBool("play", false);
                cog_2Animation.SetBool("play", false);
                cog_3Animation.SetBool("play", false);
                cog_4Animation.SetBool("play", false);
            }
            else if (_selection.CompareTag("Door"))
            {
                //Door
                doorAnimator.SetBool("play", false);
            }
            else
            {
                var HighlightedRenderer = _selection.GetComponent<Collider>();
            }
            _selection = null;
        }

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag("Door"))
            {
                //audioSource.PlayOneShot(click, 0.5f);
                doorAnimator.SetBool("play", true);
                if (Input.GetMouseButtonDown(0)) Application.Quit();
                _selection = selection;
            }
            /*
            if (selection.CompareTag("Score Board"))
            {
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(3);
                _selection = selection;
            }
            
            if (selection.CompareTag("Credit"))
            {
                //audioSource.PlayOneShot(click, 0.5f);
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(2);
                _selection = selection;
            }
            */
            if (selection.CompareTag("Setting"))
            {
                //audioSource.PlayOneShot(click, 0.5f);
                var selectionCollider = selection.GetComponent<Collider>();
                cogAnimation.SetBool("play", true);
                cog_2Animation.SetBool("play", true);
                cog_3Animation.SetBool("play", true);
                cog_4Animation.SetBool("play", true);
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(1);
                _selection = selection;

            }
            /*
            if (selection.CompareTag("Illustration"))
            {
                //audioSource.PlayOneShot(click, 0.5f);
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(1);
                _selection = selection;
            }

            if (selection.CompareTag("Donate"))
            {
                //audioSource.PlayOneShot(click, 0.5f);
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(1);
                _selection = selection;
            }
            */

            if (selection.CompareTag("StartGame"))
            {
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(2);
                _selection = selection;
            }

            /*
            if (selection.CompareTag("Tutorial"))
            {
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(3);
                _selection = selection;
            }

            if (selection.CompareTag("Co-op Mode"))
            {
                var selectionCollider = selection.GetComponent<Collider>();
                if (Input.GetMouseButtonDown(0)) SceneManager.LoadScene(1);
                _selection = selection;
            }
            */
        }
    }

    /*
    private void ChangeShader(Collider obj, string shaderType)
    {
        if (!obj) return;
        try
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            Shader OutlineShader = Shader.Find(shaderType);
            renderer.material.shader = OutlineShader;
        }
        catch (MissingComponentException)
        {
            Debug.Log(obj.name + " Missing Component");
        }
    }
    */
}
