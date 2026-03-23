using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance { get; private set; }

    [Space(10), Header("Mana Settings")]
    [SerializeField] private float _mana = 0f;
    public float Mana
    {
        get { return _mana; }
        set
        {
            _mana = value;
            _mana = Mathf.Clamp(_mana, 0f, MaxMana);

            UpdateManaUI();
        }
    }
    public float MaxMana = 100f;
    public float ManaPerTick = 5f;
    public float ManaTickTimer = 1f;
    private float _manaTimer;

    public Image ManaFillImage;
    public TextMeshProUGUI ManaText;
    public float ManaProgress => Mana / MaxMana;


    [Space(10), Header("SpellQueue")]
    public List<Element> ElementQueue = new(5);
    public bool IsElementQueueEmpty => ElementQueue.Count <= 0;
    public RectTransform ElementQueueParent;
    public List<Image> ElementImages;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Optional: keep this across scene loads
        // DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        UpdateManaUI();
        ClearElementQueue();
    }

    // Update is called once per frame
    private void Update()
    {
        GenerateMana();
    }

    private void GenerateMana()
    {
        // Check if mana is max
        if (Mana >= MaxMana)
        {
            if(_manaTimer != 0f)
                _manaTimer = 0f;
            return;
        }            

        // Give mana every x ticks
        _manaTimer += Time.deltaTime;
        if (_manaTimer >= ManaTickTimer)
        {
            Mana += ManaPerTick;
            if (Mana > MaxMana)
                Mana = MaxMana;

            _manaTimer -= ManaTickTimer;
        }
    }
    private void UpdateManaUI()
    {
        ManaText.text = $"Mana: {Mana}";
        ManaFillImage.fillAmount = ManaProgress;
    }

    public void AddElementToQueue(Element element)
    {
        // Check mana first
        if (element.ManaCost > Mana)
        {
            Debug.Log($"Not enough mana to queue {element}");
            return;
        }

        bool queueFull = ElementQueue.Count >= ElementImages.Count;

        // If queue is full, allow the add only if the incoming element can combine
        if (queueFull)
        {
           

            Debug.LogError("Trying to add element when queue is full");
            return;
        }

        if (element is ElementCancelable elementCombFull)
        {
            if (elementCombFull.TryCheckIfCancelable(out Element elementToRemove))
            {
                // Spend mana, remove matched element and add the combined element
                Mana -= element.ManaCost;

                int removeIndexFull = ElementQueue.IndexOf(elementToRemove);
                if (removeIndexFull >= 0)
                {
                    ElementQueue.RemoveAt(removeIndexFull);
                }

                RefreshElementQueueUI();
                return;
            }
        }

        // Not full: proceed normally
        Mana -= element.ManaCost;

        AddToQueue(element);
        RefreshElementQueueUI();
    }

    private void AddToQueue(Element element)
    {
        ElementQueue.Add(element);        
        Image image = ElementImages[ElementQueue.Count - 1];
        image.sprite = element.ElementSprite;
        image.gameObject.SetActive(true);

        Debug.Log($"Queued element: {element}");
    }

    public SpellData CastElementQueue()
    {
        SpellData spellData = new SpellData();

        foreach(Element element in ElementQueue)
        {
            element.ApplyElement(spellData);
        }

        ClearElementQueue();

        return spellData;
    }

    private void ClearElementQueue()
    {
        if(ElementQueue.Count > 0)
            ElementQueue.Clear();

        foreach (var element in ElementImages)
        {
            element.sprite = null;
            element.gameObject.SetActive(false);
        }
    }

    // Ensures ElementImages reflect ElementQueue order; clears unused slots
    private void RefreshElementQueueUI()
    {
        for (int i = 0; i < ElementImages.Count; i++)
        {
            if (i < ElementQueue.Count)
            {
                ElementImages[i].sprite = ElementQueue[i].ElementSprite;
                ElementImages[i].gameObject.SetActive(true);
            }
            else
            {
                ElementImages[i].sprite = null;
                ElementImages[i].gameObject.SetActive(false);
            }
        }
    }
}
