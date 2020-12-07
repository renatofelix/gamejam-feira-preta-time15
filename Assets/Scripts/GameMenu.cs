using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [DefaultExecutionOrder(100)]
    public class GameMenu : MonoBehaviour
    {
        public static GameMenu instance;

        public static string[] socialClassNames =
        {
            "E",
            "D",
            "C",
            "B",
            "A",
        };

        public static string[] educationNames =
        {
            "N/A",
            "Fundamental",
            "Médio",
            "Superior",
        };

        public static string[] budgetNames =
        {
            "Baixa",
            "Média",
            "Alta"
        };

        public static string[] meterNames =
        {
            "Baixo",
            "Médio",
            "Alto"
        };

        public static string[] efficiencyNames =
        {
            "Baixa",
            "Média",
            "Alta",
            "Super",
        };

        public static string[] genderNames =
        {
            "Homem Cis",
            "Mulher Cis",
            "Homen Trans",
            "Mulher Trans",
            "Não-binário",
            "Outro",
        };

        public static string[] raceNames =
        {
            "Branco",
            "Preto",
            "Pardo",
            "Amarelo",
            "Indígena",
            "Outro",
        };


        public Color[] raceIconColor =
        {

        };

        //Prefabs
        [Header("Prefabs")]
        public UIStructureButton structureButtonPrefab;
        public UIPersonButton personButtonPrefab;

        public Structure[] structurePrefabs;

        //References
        [Header("Static")]
        public Sprite maleIcon;
        public Sprite femaleIcon;
        public Sprite nonBinaryIcon;
        // public Sprite[] happinessIcons;

        [Header("Resources/Date")]
        public TextMeshProUGUI moneyValueLabel;
        public TextMeshProUGUI administrativePointsValueLabel;
        public TextMeshProUGUI approvalValueLabel;
        public TextMeshProUGUI populationValueLabel;
        public TextMeshProUGUI dateLabel;

        [Header("Structures/Tools")]
        public RectTransform structureSection;
        public Button sellOrDestroyButton;

        [Header("Tax")]
        public RectTransform taxPanel;
        public TextMeshProUGUI[] taxValues;
        public Button[] taxIncreaseButtons;
        public Button[] taxDecreaseButtons;

        [Header("Info")]
        public RectTransform infoPanel;
        public TextMeshProUGUI infoTitleLabel;

        public RectTransform structureInfoSection;
        public Button budgetIncreaseButton;
        public Button budgetDecreaseButton;
        public TextMeshProUGUI budgetLabel;
        public TextMeshProUGUI upkeepLabel;
        public TextMeshProUGUI profitLabel;
        public TextMeshProUGUI structureSocialClassLabel;
        public TextMeshProUGUI ownerLabel;
        public UIPersonButton ownerButton;

        public RectTransform workplaceInfoSection;
        public TextMeshProUGUI efficiencyLabel;
        public TextMeshProUGUI requiredEducationLabel;
        public TextMeshProUGUI jobCapacityLabel;
        public RectTransform workersSection;

        public RectTransform hospitalInfoSection;
        public TextMeshProUGUI bedCapacityLabel;
        public RectTransform patientsSection;

        public RectTransform schoolInfoSection;
        public TextMeshProUGUI targetEducationLabel;
        public TextMeshProUGUI studentCapacityLabel;
        public RectTransform studentsSection;

        public RectTransform personInfoSection;
        public TextMeshProUGUI ageLabel;
        public TextMeshProUGUI genderLabel;
        public TextMeshProUGUI raceLabel;
        public TextMeshProUGUI personSocialClassLabel;
        public TextMeshProUGUI educationLabel;
        public TextMeshProUGUI schoolLabel;
        public TextMeshProUGUI jobLabel;
        public TextMeshProUGUI happinessLabel;
        public TextMeshProUGUI relationshipLabel;
        public UIPersonButton relationshipButton;
        public RectTransform parentsSection;
        public RectTransform childrenSection;

        [Header("Tooltip")]
        public RectTransform tooltipPanel;
        public TextMeshProUGUI tooltipTitleLabel;
        public TextMeshProUGUI tooltipPriceLabel;
        public TextMeshProUGUI tooltipDescriptionLabel;

        [Header("Internal")]
        public Structure selectedStructure = null;
        public Person selectedPerson = null;

        public void Awake()
        {
            if(instance)
            {
                Destroy(instance);

                return;
            }

            instance = this;
        }

        public void Start()
        {
            GridManager.Instance.OnSelectTile = (Structure structure) =>
            {
                ShowStructureInfo(structure);
            };

            GridManager.Instance.OnDeselectTile = (Structure structure) =>
            {
                HideInfo();
            };

            foreach(Structure structure in structurePrefabs)
            {
                UIStructureButton button = Instantiate(structureButtonPrefab, structureSection, false);
                button.structure = structure;
            }

            for(int i = 0; i < taxValues.Length; ++i)
            {
                int index = i;

                taxValues[i].text = meterNames[(int)GameManager.instance.tax[i]];

                taxIncreaseButtons[i].onClick.AddListener(delegate()
                {
                    if(GameManager.instance.tax[index] < Meter.Count - 1)
                    {
                        ++GameManager.instance.tax[index];

                        taxValues[index].text = meterNames[(int)GameManager.instance.tax[index]];
                    }
                });

                taxDecreaseButtons[i].onClick.AddListener(delegate()
                {
                    if(GameManager.instance.tax[index] > 0)
                    {
                        --GameManager.instance.tax[index];

                        taxValues[index].text = meterNames[(int)GameManager.instance.tax[index]];
                    }
                });
            }
        }

        public void Update()
        {
            moneyValueLabel.text = GameManager.instance.money.ToString();
            // administrativePointsValueLabel.text = GameManager.instance.politicalPoints.ToString();
            approvalValueLabel.text = GameManager.instance.GetApprovalRating().ToString();
            populationValueLabel.text = GameManager.instance.city.people.Count.ToString();
            dateLabel.text = GameManager.months[GameManager.instance.month] + ", " + GameManager.instance.year.ToString();

            if(Input.GetMouseButton(1))
            {
                HideTooltip();
                HideInfo();
                HideTaxPanel();

                GridManager._instance.BuildModeOff();
            }
        }
        
        public void ShowStructureInfo(Structure structure)
        {
            infoPanel.gameObject.SetActive(true);
            structureInfoSection.gameObject.SetActive(true);

            selectedStructure = structure;

            infoTitleLabel.text = structure.displayName;
            budgetLabel.text = budgetNames[(int)structure.budget];
            upkeepLabel.text = structure.upkeepCost.ToString();
            // profitLabel.text = structure.GetProfit().ToString();
            if(structure.ownership == Ownership.Public)
            {
                ownerLabel.gameObject.SetActive(true);
                ownerLabel.text = "Governo";

                ownerButton.gameObject.SetActive(false);
            }
            else if(structure.ownership == Ownership.Public)
            {
                ownerLabel.gameObject.SetActive(false);
                
                ownerButton.gameObject.SetActive(true);
                ownerButton.SetupButton(structure.owner);
            }

            if(structure.GetType().IsSubclassOf(typeof(Workplace)))
            {
                workplaceInfoSection.gameObject.SetActive(true);

                Workplace workplace = (Workplace)structure;

                efficiencyLabel.text = efficiencyNames[(int)workplace.efficiency];
                requiredEducationLabel.text = educationNames[(int)workplace.work[0].educationRequired];
                jobCapacityLabel.text = workplace.work[0].maxWorkers.ToString();

                foreach(RectTransform transform in workersSection)
                {
                    Destroy(transform);
                }

                foreach(Person person in workplace.work[0].workers)
                {
                    CreatePersonButton(workersSection, person);
                }
            }
            else
            {
                workplaceInfoSection.gameObject.SetActive(false);
            }

            if(structure.GetType().IsSubclassOf(typeof(Hospital)))
            {
                hospitalInfoSection.gameObject.SetActive(true);

                Hospital hospital = (Hospital)structure;

                bedCapacityLabel.text = hospital.maxPatients.ToString();

                foreach(RectTransform transform in patientsSection)
                {
                    Destroy(transform);
                }

                foreach(Person person in hospital.patients)
                {
                    CreatePersonButton(patientsSection, person);
                }
            }
            else
            {
                hospitalInfoSection.gameObject.SetActive(false);
            }

            if(structure.GetType().IsSubclassOf(typeof(School)))
            {
                schoolInfoSection.gameObject.SetActive(true);

                School hospital = (School)structure;

                studentCapacityLabel.text = hospital.maxStudents.ToString();

                foreach(RectTransform transform in studentsSection)
                {
                    Destroy(transform);
                }

                foreach(Person person in hospital.students)
                {
                    CreatePersonButton(studentsSection, person);
                }
            }
            else
            {
                schoolInfoSection.gameObject.SetActive(false);
            }
        }

        public void OnClickIncreaseBudget()
        {
            if(selectedStructure.budget < Budget.Count - 1)
            {
                ++selectedStructure.budget;

                budgetLabel.text = budgetNames[(int)selectedStructure.budget];
            }
        }

        public void OnClickDecreaseBudget()
        {
            if(selectedStructure.budget > 0)
            {
                --selectedStructure.budget;

                budgetLabel.text = budgetNames[(int)selectedStructure.budget];
            }
        }

        public Sprite GetPersonIcon(Person person)
        {
            if(person.gender == Gender.CisWoman || person.gender == Gender.TransWoman)
            {
                return maleIcon;
            }
            else if(person.gender == Gender.CisMan || person.gender == Gender.TransMan)
            {
                return femaleIcon;
            }
            else
            {
                return nonBinaryIcon;
            }
        }

        public UIPersonButton CreatePersonButton(RectTransform parent, Person person)
        {
            UIPersonButton button = Instantiate(personButtonPrefab, parent, false);
            button.SetupButton(person);
            
            return button;
        }

        public void ShowPersonInfo(Person person)
        {
            infoPanel.gameObject.SetActive(true);
            personInfoSection.gameObject.SetActive(true);

            structureInfoSection.gameObject.SetActive(false);
            workplaceInfoSection.gameObject.SetActive(false);
            hospitalInfoSection.gameObject.SetActive(false);
            schoolInfoSection.gameObject.SetActive(false);
    
            selectedPerson = person;

            infoTitleLabel.text = person.name;
            ageLabel.text = person.age.ToString();
            genderLabel.text = genderNames[(int)person.gender];
            raceLabel.text = raceNames[(int)person.colorOrRace];
            personSocialClassLabel.text = socialClassNames[(int)person.socialClass];
            educationLabel.text = educationNames[(int)person.education];

            if(person.school != null)
            {
                schoolLabel.text = person.school.name;
            }
            else
            {
                schoolLabel.text = "N/A";
            }

            if(person.job != null)
            {
                jobLabel.text = person.job.name;
            }
            else
            {
                schoolLabel.text = "N/A";
            }

            happinessLabel.text = person.happiness.ToString();

            if(person.relationshipPartner == null)
            {
                relationshipLabel.gameObject.SetActive(true);
                relationshipLabel.text = "N/A";

                relationshipButton.gameObject.SetActive(false);
            }
            else
            {
                relationshipLabel.gameObject.SetActive(false);
                
                relationshipButton.gameObject.SetActive(true);
                relationshipButton.SetupButton(person.relationshipPartner);
            }

            foreach(RectTransform transform in parentsSection)
            {
                Destroy(transform);
            }

            foreach(Person parent in person.parents)
            {
                CreatePersonButton(patientsSection, parent);
            }

            foreach(RectTransform transform in childrenSection)
            {
                Destroy(transform);
            }

            foreach(Person child in person.children)
            {
                CreatePersonButton(patientsSection, child);
            }
        }

        public void HideInfo()
        {
            infoPanel.gameObject.SetActive(false);

            selectedStructure = null;
            selectedPerson = null;
        }

        public void ShowTaxPanel()
        {
            taxPanel.gameObject.SetActive(true);
        }

        public void HideTaxPanel()
        {
            taxPanel.gameObject.SetActive(false);
        }

        public void ShowTooltip(Vector2 position, string name, string price, string description)
        {
            tooltipPanel.gameObject.SetActive(true);
            tooltipPanel.transform.position = position;

            tooltipTitleLabel.text = name;
            tooltipPriceLabel.text = price;
            tooltipDescriptionLabel.text = description;
        }

        public void HideTooltip()
        {
            tooltipPanel.gameObject.SetActive(false);
        }

        public void OnClickSellOrDestroy()
        {
            GridManager._instance.ActivateModeSale();
        }

        public void OnClickTax()
        {
            if(taxPanel.gameObject.activeSelf)
            {
                HideTaxPanel();
            }
            else
            {
                ShowTaxPanel();
            }
        }
    }
}