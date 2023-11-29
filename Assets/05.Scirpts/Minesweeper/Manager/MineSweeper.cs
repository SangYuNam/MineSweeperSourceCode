using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.System;

namespace MineSweeper.Framework.game
{
    public enum EBlockIndex
    {
        Mine = -1, // 지뢰(-1)
        Space, // 빈공간(0)
        Number, // 숫자칸(1)
    }

    public class MineSweeper : MonoBehaviour
    {
        #region 필드 변수

        // 오브젝트 참조
        [Header("오브젝트 모음")]
        [SerializeField] private GameObject m_blockPrefab = null; // 블럭 오브젝트의 프리팹
        [SerializeField] private GameObject m_camera = null; // 카메라의 프리팹

        [Header("배열 모음")]
        [SerializeField] private Material[] m_mats; // 메테리얼 저장 배열

        // int형 변수
        private int m_mineCount = 0; // 지뢰 갯수
        private int m_blockClickCount = 0; // 몇개의 블럭을 클릭했는지 확인할 변수
        private int m_width = 0; // 가로
        private int m_height = 0; // 세로
        private int m_numCount = 0; // 주변에 지뢰 갯수를 저장할 변수

        // float형 변수
        private float m_shakeDuration = 0.2f;
        private float m_shakePower = 0.1f;

        // bool형 변수
        public bool m_possibleClick = true; // 클릭이 가능한 상태인지?

        // 참조형 변수
        private Block m_ClickBlock = null; // 클릭한 Block 오브젝트의 참조값을 넣을 Block 클래스 변수
        private MeshRenderer m_clickMesh = null; // 클릭한 Block 오브젝트의 MeshRenderer를 저장할 변수

        // 배열
        private List<List<GameObject>> m_blockObjects = new List<List<GameObject>>(); // 오브젝트를 참조하기위해 캐싱해두는 배열
        private List<Block> m_blocks = new List<Block>(); // Block 스크립트를 참조하기위해 캐싱해두는 배열
        private List<MeshRenderer> m_meshs = new List<MeshRenderer>(); // 오브젝트의 MeshRenderer를 참조하기위해 캐싱해두는 배열
        private int[,] m_maps = null; // 맵에 블럭을 배치 전 Mine, Space, Number 확인을 관리할 map 2차원 배열
        private int[,] m_aroundMineCounts = null; // 주변에 폭탄이 있는지 확인하고 값을 저장할 2차원 배열

        // 카메라 고정 위치값
        private Vector3 m_cameraOriginPos;

        #endregion


        #region 유니티 고유 이벤트

        private void Start()
        {
            // GameManager m_mineSweeper에 자신의 인스턴스 할당
            GameManager.Instance.m_MineSweeper = this;

            // 게임시작
            StartGame();

            // 카메라 Shake를 위한 카메라 포지션 값 저장
            m_cameraOriginPos = m_camera.transform.position;
        }

        private void Update()
        {

            // 맵전체를 밝혀주고 Clear 시켜주는 기능
            if (Input.GetKeyDown(KeyCode.F1) && GameManager.Instance.GameState == EGameState.Play)
            {
                AllClear();
                GameManager.Instance.ChangeState(EGameState.Clear);
            }

            // Block을 눌렀을 시 Block이 무엇인지 판단하고 열어주는 기능
            if (Input.GetMouseButtonDown(0) && m_possibleClick)
            {
                ClickBlock();
            }

            // Block에 폭탄을 암시하는 깃발을 생성해주는 기능
            if (Input.GetMouseButtonDown(1))
            {
                CreateFlag();
            }

        }

        #endregion


        #region 함수

        // 난이도 셋팅 함수
        public void SetDifficulty(EGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case EGameDifficulty.None:
                    {
                        Debug.Log("difficulty is None");
                        break;
                    }

                // 초급 난이도 셋팅
                case EGameDifficulty.Beginner:
                    {
                        m_width = 9;
                        m_height = 9;
                        m_mineCount = 10;
                        m_blockClickCount = (m_width * m_height) - m_mineCount;
                        m_camera.transform.position = new Vector3(4f, 4.2f, -9f);
                        break;
                    }

                // 중급 난이도 셋팅
                case EGameDifficulty.Intermediate:
                    {
                        m_width = 16;
                        m_height = 16;
                        m_mineCount = 40;
                        m_blockClickCount = (m_width * m_height) - m_mineCount;
                        m_camera.transform.position = new Vector3(8.5f, 8.5f, -18f);
                        break;
                    }

                // 고급 난이도 셋팅
                case EGameDifficulty.Expert:
                    {
                        m_width = 30;
                        m_height = 16;
                        m_mineCount = 99;
                        m_blockClickCount = (m_width * m_height) - m_mineCount;
                        m_camera.transform.position = new Vector3(15f, 8f, -20f);
                        break;
                    }

                default:
                    {
                        Debug.Log("Have Not Difficulty");
                        break;
                    }
            }
        }

        // 맵 전체를 밝혀주고 Clear 시켜주는 함수
        public void AllClear()
        {
            // 모든 블럭을 순회
            for(int i = 0; i < m_width * m_height; i++)
            {
                // m_blocks[0] ~ m_blocks[m_width * m_height] 인덱스 값을 받음
                switch(m_blocks[i].Index)
                {
                    // 지뢰 인덱스(-1) 값을 갖고 있을 경우
                    case (int)EBlockIndex.Mine:
                        {
                            m_meshs[i].material.color = Color.red;
                            break;
                        }

                    // 빈공간 인덱스(0) 값을 갖고 있을 경우
                    case (int)EBlockIndex.Space:
                        {
                            m_meshs[i].material.color = Color.gray;
                            break;
                        }

                    // 숫자 인덱스(1) 값을 갖고 있을 경우
                    case (int)EBlockIndex.Number:
                        {
                            // Mathf.Clamp 함수로 최소, 최대값을 정해줘 예외처리
                            int matIndex = Mathf.Clamp(m_blocks[i].NumCount - 1, 0, m_mats.Length - 1);
                            m_meshs[i].material = m_mats[matIndex];
                            break;
                        }

                    default:
                        {
                            Debug.Log("Strange index number");
                            break;
                        }
                }
            }
            m_possibleClick = false; // 클릭이 불가능한 상태로 변환
        }

        // Block 을 클릭 시 Block의 Index를 판별하고 이미지를 바꿔주는 함수
        private void ClickBlock()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            SoundManager.Instance?.Play("Cube", ESound.Effect);

            if (Physics.Raycast(ray, out hit))
            {
                // Getcomponent
                m_ClickBlock = hit.transform.GetComponent<Block>();
                m_clickMesh = hit.transform.GetComponent<MeshRenderer>();
                              
                // 블럭이 열려있지 않고 && 깃발이 아니라면
                if (m_ClickBlock.ChangeBlock == false && !m_ClickBlock.IsFlag)
                {
                    // 눌리는 블럭 애니메이션 구동
                    m_ClickBlock.ClickBlock();

                    // 블럭의 인덱스 값을 받아 기능을 나눔
                    switch (m_ClickBlock.Index)
                    {
                        // 블럭이 지뢰 인덱스(-1) 값을 갖고 있을 경우
                        case (int)EBlockIndex.Mine:
                            {
                                // 지뢰를 눌럿을때의 기능 함수
                                ClickMine();

                                SoundManager.Instance?.Play("Bomb", ESound.Effect);
                                break;
                            }

                        // 블럭이 빈공간(0) 인덱스 값을 갖고 있을 경우
                        case (int)EBlockIndex.Space:
                            {
                                // 빈공간을 눌렀을때의 기능 함수
                                ClickSpace();

                                // 카메라 Shake
                                StartCoroutine(ShakingCamera(m_shakePower,m_shakeDuration));

                                SoundManager.Instance?.Play("Space", ESound.Effect);
                                break;
                            }

                        // 블럭이 숫자(1) 인덱스 값을 갖고 있을 경우
                        case (int)EBlockIndex.Number:
                            {
                                // 숫자를 눌렀을때의 기능 함수
                                ClickNumber();
                                
                                SoundManager.Instance?.Play("Num", ESound.Effect);
                                break;
                            }
                            
                        default:
                            {
                                Debug.Log("Strange index number");
                                break;
                            }
                    }
                    // 이제 이 블럭은 열린 블럭이다.
                    m_ClickBlock.ChangeBlock = true; 
                }
            }
        }

        // 지뢰를 클릭했다면?
        private void ClickMine()
        {
            // 블럭의 색을 지뢰색(빨강)으로 변경 후
            m_clickMesh.material.color = Color.red;

            // 게임의 상태를 패배로 변경
            GameManager.Instance.ChangeState(EGameState.Defeat);

            //클릭 불가능
            m_possibleClick = false;
        }

        // 빈공간을 클릭했다면?
        private void ClickSpace()
        {
            // 블럭의 색을 빈공간(회색)으로 변경 후
            m_clickMesh.material.color = Color.gray;

            //주변 빈공간과 인접한 숫자칸까지 밝혀주는 함수 실행
            OpenSpaceRecursive(m_ClickBlock.Width, m_ClickBlock.Height);

            // m_clickCount의 숫자를 올려주는 함수 실행
            CountClickBlock();
        }

        // 숫자를 클릭했다면?
        private void ClickNumber()
        {
            // 0이 나왔을 경우와 m_block.NumCount에 음수,8초과의 값이 들어갔을 경우의 예외처리
            int matIndex = Mathf.Clamp(m_ClickBlock.NumCount - 1, 0, m_mats.Length - 1); 

            // 위에서 나온 matIndex 값을 대입해 그림 변경
            m_clickMesh.material = m_mats[matIndex];

            // m_clickCount의 숫자를 올려주는 함수 실행
            CountClickBlock();
        }

        // 깃발을 생성해주는 함수
        private void CreateFlag()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // GetComponenet
                m_ClickBlock = hit.transform.GetComponent<Block>();
                m_clickMesh = hit.transform.GetComponent<MeshRenderer>();

                // 블럭이 열린 블럭이 아니라면
                if (!m_ClickBlock.ChangeBlock)
                {
                    // 블럭에 깃발이 안꽂혀있다면 깃발 생성
                    if (!m_ClickBlock.IsFlag)
                    {
                        m_clickMesh.material = m_mats[8];
                        m_ClickBlock.IsFlag = true;
                    }

                    // 깃발이 꽂혀있다면 깃발 제거
                    else
                    {
                        m_clickMesh.material = m_mats[9];
                        m_ClickBlock.IsFlag = false;
                    }
                }
            }
        }

        // 난이도에 따라서 게임을 시작하는 함수
        private void StartGame()
        {
            // 난이도 설정
            SetDifficulty(GameManager.Instance.GameDifficulty);

            // 난이도에 맞는 블럭생성
            InitBlock();
        }

        // 게임 시작 시 Block 생성해주는 함수
        private void InitBlock()
        {
            // m_blocks의 인덱스 값
            int indexCount = 0; 

            // 배열 초기화
            m_maps = new int[m_width, m_height]; 
            m_aroundMineCounts = new int[m_width, m_height];
            m_blockObjects.Clear();

            // 폭탄인덱스 주입
            SetMine();

            // 숫자인덱스 주입
            SetNumber();

            for (int w = 0; w < m_width; w++)
            {
                // 배열 추가
                m_blockObjects.Add(new List<GameObject>());

                for (int h = 0; h < m_height; h++)
                {
                    // 블럭 오브젝트 생성
                    GameObject blockobj = Instantiate(m_blockPrefab, new Vector3(w * 1.05f, h * 1.05f, 0), Quaternion.Euler(0,0,180));

                    // 블럭의 이름 = Block[가로,세로]
                    blockobj.name = $"Block[{w},{h}]";

                    // 배열 추가 및 캐싱
                    m_blockObjects[w].Add(blockobj);
                    m_blocks.Add(blockobj.GetComponent<Block>());
                    m_meshs.Add(blockobj.GetComponent<MeshRenderer>());

                    switch (m_maps[w, h])
                    {
                        // 배열이 지뢰 인덱스(-1) 값을 갖고 있을 경우
                        case (int)EBlockIndex.Mine: 
                            {
                                // 블록 스크립트의 index에 폭탄 인덱스 주입
                                m_blocks[indexCount].Index = (int)EBlockIndex.Mine;
                                break;
                            }

                        // 배열이 빈공간 인덱스(0) 값을 갖고 있을 경우
                        case (int)EBlockIndex.Space: 
                            {
                                // 이후 빈공간을 밝혀줄 재귀 함수에서 파라매터 값으로 사용될 블록 스크립트의 가로 세로값 주입
                                m_blocks[indexCount].Width = w;
                                m_blocks[indexCount].Height = h;
                                break;
                            }

                        // 배열이 숫자 인덱스(1) 값을 갖고 있을 경우
                        case (int)EBlockIndex.Number: 
                            {
                                // 블록 스크립트의 index에 숫자 인덱스 및 주변 지뢰 NumCount 주입
                                m_blocks[indexCount].Index = (int)EBlockIndex.Number;
                                m_blocks[indexCount].NumCount = m_aroundMineCounts[w, h];
                                break;
                            }

                        default:
                            {
                                Debug.Log("m_maps Index is Out of Range");
                                break;
                            }
                    }
                    indexCount++;
                }
            }
        }

        // m_maps 배열에 폭탄 인덱스를 생성하는 함수
        private void SetMine()
        {
            // 랜덤한 값을 뽑아줄 List 생성
            List<int> randomLists = new List<int>(); 

            // 가로,세로
            int width = 0;
            int height = 0;

            for (int e = 0; e < m_width * m_height; e++)
            {
                // 블럭 갯수 만큼 Add
                randomLists.Add(e); 
            }

            // mineCount 만큼만 반복
            for (int i = 0; i < m_mineCount; i++) 
            {
                // 1. 랜덤값을 뽑고
                int randomNumber = UnityEngine.Random.Range(0, randomLists.Count);

                // 2. randomLists[랜덤값]의 인덱스 값을 저장
                int randomIndex = randomLists[randomNumber];

                // 3. randomList[랜덤값]을 삭제
                randomLists.RemoveAt(randomNumber);

                // 4. 연산을 통한 랜덤한 width, height 값을 저장
                width = randomIndex % m_width;
                height = randomIndex / m_width;

                // 5. 랜덤으로 뽑은 자리에 -1(지뢰) 인덱스 값을 저장
                m_maps[width, height] = -1;
            }
        }

        // width, height값을 전달 받고 주변 8개의 블럭에 있는 지뢰 수 만큼 카운트를 올려주는 함수
        private void CheckMine(int width, int height)
        {
            // 색인 변수 i를 -1, 0, 1 세번 반복
            for (int i = -1; i <= 1; i++)
            {
                // 색인 변수 u를 -1, 0, 1 세번 반복
                for (int u = -1; u <= 1; u++)
                {
                    // 지뢰 블럭, 현재 자기 자신 이라면 continue
                    if (m_maps[width, height] == -1 || i == 0 && u == 0)
                    {
                        continue;
                    }

                    // width, height 대입 전에 배열 값이 초과되거나 음수가 나올경우 continue
                    if (width + i < 0 || width + i >= m_width || height + u < 0 || height + u >= m_height)
                    {
                        continue;
                    }

                    // 지뢰가 있을 경우 m_numCount++ 
                    m_numCount = m_maps[width + i, height + u] == -1 ? m_numCount + 1 : m_numCount;

                }
            }
        }

        // m_aroundMineCount 배열에 Block 주변의 지뢰 개수를 값으로 넣어주고 m_maps 배열에 Number Block의 Index 값을 넣어주는 함수
        private void SetNumber()
        {
            // m_width * m_height = 전체 블럭 갯수 만큼 반복
            for (int wh = 0; wh < m_width * m_height; wh++)
            {
                // 가로,세로값
                int width = wh % m_width;
                int height = wh / m_width;

                // 주변에 지뢰가 있을경우 갯수만큼 m_numCount를 올려주는 함수
                CheckMine(width, height);

                // m_numCount가 1이상일 경우 숫자(1) 인덱스를 주입
                m_maps[width, height] = m_numCount >= 1 ? 1 : m_maps[width, height];

                // 주변에 지뢰가 몇개인지도 m_aroundMineCounts에 저장
                m_aroundMineCounts[width, height] = m_numCount;

                // 카운트 초기화
                m_numCount = 0;
            }
        }

        // 빈공간(0)을 눌렀을 때 주변의 빈공간 전부와 인접한 숫자 블럭을 열어주는 함수
        private void OpenSpaceRecursive(int width, int height)
        {
            // m_maps[width,height]를 m_blocks[?]로 참조할때 ?에 들어가야 할 값
            int blocksIndex = m_blockObjects[0].Count * width + height;

            // m_maps[width,height]를 m_meshsIndex[?]로 참조할때 ?에 들어가야 할 값
            int meshsIndex = m_blockObjects[0].Count * width + height;

            // 인덱스 값이 벗어나면 return
            if (width < 0 || width >= m_width || height < 0 || height >= m_height) 
            {
                return;
            }

            // 이미 열려있는 Block인지? 빈공간(0) 인덱스가 맞는지?
            if (m_blocks[blocksIndex].ChangeBlock == true || m_maps[width, height] != 0) 
            {
                return;
            }

            // 블럭을 빈공간 블럭 회색으로 변경
            m_meshs[meshsIndex].material.color = Color.gray;

            // 이제 이블럭은 바뀐 블럭(클릭, 깃발 X)
            m_blocks[blocksIndex].ChangeBlock = true;

            // ClickCount++
            CountClickBlock();

            // 열린 블럭의 주변을 다시 탐색
            ResearchBlock(width, height);

        }

        // OpenSpaceRecursive 재귀함수를 인접 타일 수 8 만큼 반복 실행해주는 역할
        private void ResearchBlock(int width, int height)
        {
            // 색인 변수 i를 -1, 0, 1 세번 반복
            for (int i = -1; i <= 1; i++)
            {
                // 색인 변수 u를 -1, 0, 1 세번 반복
                for (int j = -1; j <= 1; j++)
                {
                    // 주변의 탐색을 위해 주변 블럭의 width, height값을 따로 변수 선언
                    int width_x = width + i;
                    int height_x = height + j;

                    // 자기 자신이라면 continue
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    // 검사 후 빈공간이 맞다면 빈공간 블럭으로 바꿔주는 재귀함수 호출
                    OpenSpaceRecursive(width_x, height_x);

                    // 빈칸 옆 숫자 열기
                    OpenNearNumber(width_x, height_x);
                }
            }
        }

        // 빈칸에 인접한 숫자 블럭을 열어주는 함수
        private void OpenNearNumber(int width_x, int height_x)
        {
            // m_maps[width_x,height_x]를 m_blocks[?]로 참조할때 ?에 들어가야 할 값
            int blocksIndex = m_blockObjects[0].Count * width_x + height_x;

            // m_maps[width_x,height_x]를 m_meshsIndex[?]로 참조할때 ?에 들어가야 할 값
            int meshsIndex = m_blockObjects[0].Count * width_x + height_x;

            // 인덱스 값이 벗어나면 return
            if (width_x < 0 || width_x >= m_width || height_x < 0 || height_x >= m_height)
            {
                return;
            }

            // 이미 열려있는 Block인지? 빈공간(0) 인덱스가 맞는지?
            if (m_blocks[blocksIndex].Index != 1 || m_blocks[blocksIndex].ChangeBlock)
            {
                return;
            }

            // 해당 오브젝트의 Block스크립트에서 NumCount를 가져와 지역 변수 선언 후 대입
            int numCount = m_blocks[blocksIndex].NumCount;

            // 주변 인접한 지뢰갯수가 1이상이라면
            if (numCount > 0)
            {
                // 오브젝트의 material에 m_mats[인접한 지뢰갯수 -1(숫자 메테리얼이 0 부터 시작)] 대입
                m_meshs[meshsIndex].material = m_mats[numCount - 1];
            }
            else
            {
                return;
            }

            // ClickCount++
            CountClickBlock();

            // 이제 이블럭은 바뀐 블럭(클릭, 깃발 X)
            m_blocks[blocksIndex].ChangeBlock = true;

            // 열린 블럭 주변을 재귀함수로 다시 탐색
            OpenSpaceRecursive(width_x, height_x);
            
        }

        // 클리어 조건을 확인 해 주는 함수
        private void CountClickBlock()
        {
            // m_clickBlock 값이 0 보다 크다면
            if (m_blockClickCount > 0)
            {
                // m_clickBlock 값을 1 줄인다.
                --m_blockClickCount;
            }

            // m_clickBlock 값이 0이거나 0보다 작다면
            else
            {
                // 게임의 상태를 승리로 변경
                GameManager.Instance.ChangeState(EGameState.Clear);
            }
        }                

        // 카메라 Shake 코루틴
        public IEnumerator ShakingCamera(float power, float duration)
        {
            // timer 초기화
            float timer = 0;

            // timer가 매개변수로 설정한 지속시간보다 작다면
            while (timer <= duration)
            {
                // 카메라를 흔든다.
                m_camera.transform.position = UnityEngine.Random.insideUnitSphere * power + m_cameraOriginPos;

                // timer는 시간증가
                timer += Time.deltaTime;

                yield return null;
            }
            // 끝났으면 카메라 포지션은 다시 처음 포지션으로
            m_camera.transform.position = m_cameraOriginPos;
        }

        #endregion


        #region 재정의 함수
        #endregion


        #region 인터페이스 구현
        #endregion

    }
}
