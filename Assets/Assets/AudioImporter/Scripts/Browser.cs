using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;  // For LINQ methods (optional)
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

/// <summary>
/// A file browser.
/// </summary>
public class Browser : MonoBehaviour
{
    /// <summary>
    /// Occurs when a file has been selected in the browser.
    /// </summary>
    public event Action<string> FileSelected;

    /// <summary>
    /// File extensions that will show up in the browser.
    /// </summary>
    public List<string> extensions;

    public GameObject listItemPrefab;
    public GameObject upButton;
    public ScrollRect scrollRect;
    public GameObject folderPanel;
    public GameObject filePanel;

    private string currentDirectory;
    private string[] drives;
    private List<string> directories;
    private List<string> files;
    private bool selectDrive;
    private bool scrolling;

    /// <summary>
    /// Search for files by name.
    /// </summary>
    public TMP_InputField directorySearch;

    /// <summary>
    /// button for navigating to the Downloads folder.
    /// </summary>
    public Button downloadsButton;

    /// <summary>
    /// for navigating to a custom folder
    /// </summary>
    public Button goToPathButton;
    public TMP_InputField pathSearch;


    void Awake()
    {
        /// Check for a saved path so the player dosent have to enter it every time
        if (PlayerPrefs.HasKey("lastPath"))
        {
            string savedPath = PlayerPrefs.GetString("lastPath");

            if (Directory.Exists(savedPath))
            {
                currentDirectory = savedPath;
                selectDrive = false;
                pathSearch.text = savedPath;
            }
        }

        directories = new List<string>();
        files = new List<string>();

        drives = Directory.GetLogicalDrives();
        currentDirectory = PlayerPrefs.GetString("currentDirectory", "");

        selectDrive = (string.IsNullOrEmpty(currentDirectory) || !Directory.Exists(currentDirectory));

        BuildContent();

        if (directorySearch != null)
        {
            directorySearch.onValueChanged.AddListener(OnSearchInputChanged);
        }

        // Hook up the Downloads button
        if (downloadsButton != null)
        {
            downloadsButton.onClick.AddListener(GotoDownloads);
        }

        if (goToPathButton != null)
        {
            goToPathButton.onClick.AddListener(GotoPathFromInput);
        }

    }

    /// <summary>
    /// Go to the current directory's parent directory.
    /// </summary>
    public void Up()
    {
        if (currentDirectory == Path.GetPathRoot(currentDirectory))
        {
            selectDrive = true;
            ClearContent();
            BuildContent();
        }
        else
        {
            currentDirectory = Directory.GetParent(currentDirectory).FullName;
            ClearContent();
            BuildContent();
        }
    }

    private void GotoPathFromInput()
    {
        if (pathSearch == null) return;

        string path = pathSearch.text.Trim();

        if (Directory.Exists(path))
        {
            // save path
            PlayerPrefs.SetString("lastPath", path);
            PlayerPrefs.Save();        
            currentDirectory = path;
            selectDrive = false;
            ClearContent();
            BuildContent();
        }
        else
        {
            Debug.LogWarning($"Invalid path: {path}");
            //Show a UI warning or flash the input field
            pathSearch.image.color = Color.red;
            // Optionally reset after a second:
            StartCoroutine(ResetInputFieldColor());
        }
    }
    private IEnumerator ResetInputFieldColor()
    {
        yield return new WaitForSeconds(1f);
        pathSearch.image.color = Color.white;
    }

    /// <summary>
    /// Navigates directly to the Downloads folder.
    /// </summary>
    private void GotoDownloads()
    {
        string downloadsFolder = GetDownloadsFolderPath();

        if (downloadsFolder != null)
        {
            currentDirectory = downloadsFolder;
            selectDrive = false;
            ClearContent();
            BuildContent();
        }
        else
        {
            Debug.LogWarning("Downloads folder not found.");
        }
    }

    /// <summary>
    /// Get the path to the Downloads folder based on the operating system.
    /// </summary>
    private string GetDownloadsFolderPath()
    {
        string downloadsFolder = "";

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Downloads");
        }

        if (Directory.Exists(downloadsFolder))
        {
            return downloadsFolder;
        }

        return null;
    }

    /// <summary>
    /// Handles the search input for filtering files and directories.
    /// </summary>
    private void OnSearchInputChanged(string input)
    {
        StopCoroutine("FilterSearchCoroutine");

        if (string.IsNullOrWhiteSpace(input))
        {
            BuildContent();
            return;
        }

      /* StartCoroutine(FilterSearchCoroutine(input)); */
    }

    /// <summary>
    /// Filters files and directories based on the search input.
    /// </summary>
    /*private IEnumerator FilterSearchCoroutine(string input)
    {
        
        ClearContent();
        directories.Clear();
        files.Clear();

        if (selectDrive)
        {
            // Recursive search across drives (for directories only)
            foreach (string drive in drives)
            {
                if (!Directory.Exists(drive))
                    continue;

                Stack<string> stack = new Stack<string>();
                stack.Push(drive);

                int counter = 0;
                while (stack.Count > 0)
                {
                    string currentDir = stack.Pop();
                    string[] subDirs = new string[0];
                    try
                    {
                        subDirs = Directory.GetDirectories(currentDir);
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (PathTooLongException) { }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Error accessing {currentDir}: {e.Message}");
                    }

                    foreach (var dir in subDirs)
                    {
                        if (Path.GetFileName(dir).IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            directories.Add(dir);
                        }
                        stack.Push(dir);

                        counter++;
                        if (counter % 50 == 0)
                            yield return null;
                    }
                }
            }
        }
        else
        {
            // Non-drive mode: search in current directory
            try
            {
                foreach (string dir in Directory.GetDirectories(currentDirectory))
                {
                    if (Path.GetFileName(dir).IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                        directories.Add(dir);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error searching directories in {currentDirectory}: {e.Message}");
            }

            // For files, search through all files and partition into allowed and others.
            try
            {
                string[] allFiles = Directory.GetFiles(currentDirectory);
                List<string> allowedFiles = new List<string>();
                List<string> otherFiles = new List<string>();

                foreach (string file in allFiles)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (extensions.Contains(Path.GetExtension(file)))
                        {
                            allowedFiles.Add(file);
                        }
                        else
                        {
                            otherFiles.Add(file);
                        }
                    }
                }

                // If in Downloads folder, sort by last modified date descending.
                if (currentDirectory == GetDownloadsFolderPath())
                {
                    allowedFiles.Sort((a, b) => File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)));
                    otherFiles.Sort((a, b) => File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)));
                }

                // Place allowed files at the top.
                files.AddRange(allowedFiles);
                files.AddRange(otherFiles);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error searching files in {currentDirectory}: {e.Message}");
            }
        }

        StopAllCoroutines();
        StartCoroutine(refreshDirectories());
        StartCoroutine(refreshFiles());

        // Keep the input focus
        if (upButton != null && !(EventSystem.current.currentSelectedGameObject?.GetComponent<TMP_InputField>() != null))
        {
            EventSystem.current.SetSelectedGameObject(upButton);
        }

        yield break; 
} */

    /// <summary>
    /// Builds the file and directory list based on the current directory.
    /// </summary>
    private void BuildContent()
    {
        directories.Clear();
        files.Clear();

        if (selectDrive)
        {
            directories.AddRange(drives);
            // No files shown at the drive level
            directories.Sort((a, b) => Directory.GetLastWriteTime(b).CompareTo(Directory.GetLastWriteTime(a)));

            StopAllCoroutines();
            StartCoroutine(refreshDirectories());
            return;
        }

        try
        {
            directories.AddRange(Directory.GetDirectories(currentDirectory));
            foreach (string file in Directory.GetFiles(currentDirectory))
            {
                if (extensions.Contains(Path.GetExtension(file)))
                    files.Add(file);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }

        // Sort both lists by last modified (descending)
        files.Sort((a, b) => File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)));
        directories.Sort((a, b) => Directory.GetLastWriteTime(b).CompareTo(Directory.GetLastWriteTime(a)));

        // 🔁 First refresh files, then folders
        StopAllCoroutines();
        StartCoroutine(refreshFiles());
        StartCoroutine(refreshDirectories());

        if (upButton != null && !(EventSystem.current.currentSelectedGameObject?.GetComponent<TMP_InputField>() != null))
        {
            EventSystem.current.SetSelectedGameObject(upButton);
        }
    }



    /// <summary>
    /// Clears the content in the folder and file panels.
    /// </summary>
    private void ClearContent()
    {
        Button[] children = filePanel.GetComponentsInChildren<Button>();
        foreach (Button child in children)
            Destroy(child.gameObject);

        children = folderPanel.GetComponentsInChildren<Button>();
        foreach (Button child in children)
            Destroy(child.gameObject);
    }

    /// <summary>
    /// Handles file selection.
    /// </summary>
    private void OnFileSelected(int index)
    {
        string path = files[index];
        FileSelected?.Invoke(path);
        PlayerPrefs.SetString("currentDirectory", currentDirectory);
    }

    /// <summary>
    /// Handles directory selection.
    /// </summary>
    private void OnDirectorySelected(int index)
    {
        if (selectDrive)
        {
            currentDirectory = drives[index];
            selectDrive = false;
        }
        else
        {
            currentDirectory = directories[index];
        }
        ClearContent();
        BuildContent();
    }

    /// <summary>
    /// Refreshes the file panel with the new list of files.
    /// </summary>
    private IEnumerator refreshFiles()
    {
        for (int i = 0; i < files.Count; i++)
        {
            AddFileItem(i);
            yield return null;
        }
    }

    /// <summary>
    /// Refreshes the folder panel with the new list of directories.
    /// </summary>
    private IEnumerator refreshDirectories()
    {
        for (int i = 0; i < directories.Count; i++)
        {
            AddDirectoryItem(i);
            yield return null;
        }
    }

    /// <summary>
    /// Adds a file item to the file panel.
    /// </summary>
    private void AddFileItem(int index)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        Button button = listItem.GetComponent<Button>();
        button.onClick.AddListener(() => { OnFileSelected(index); });
        listItem.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileName(files[index]);
        listItem.transform.SetParent(filePanel.transform, false);
    }

    /// <summary>
    /// Adds a directory item to the folder panel.
    /// </summary>
    private void AddDirectoryItem(int index)
    {
        GameObject listItem = Instantiate(listItemPrefab);
        Button button = listItem.GetComponent<Button>();
        button.onClick.AddListener(() => { OnDirectorySelected(index); });

        if (selectDrive)
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = directories[index];
        else
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileName(directories[index]);

        listItem.transform.SetParent(folderPanel.transform, false);
    }

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null)
        {
            if (selected.transform.IsChildOf(transform))
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape))
                    Up();

                if (Mathf.Abs(Input.GetAxis("Vertical")) > .3f)
                {
                    if (selected.transform.IsChildOf(transform))
                    {
                        scrollRect.movementType = ScrollRect.MovementType.Clamped;
                        RectTransform rt = selected.GetComponent<RectTransform>();
                        Vector2 dif = scrollRect.transform.position - rt.position;

                        if (Mathf.Abs(dif.y) > .5f)
                        {
                            Vector2 scrollVelocity = Vector2.zero;
                            scrollVelocity.y = dif.y * 3;
                            scrollRect.velocity = scrollVelocity;
                        }

                        scrolling = true;
                    }
                }
                else if (scrolling)
                {
                    if (scrollRect.verticalNormalizedPosition > .99f || scrollRect.verticalNormalizedPosition < .01f)
                        scrollRect.StopMovement();
                    scrolling = false;
                }
            }
        }
    }
}