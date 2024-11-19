using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.
// using UnityEngine.Pool;
[System.Serializable]
public class game_man : SingletonMonoBehaviour<game_man>
{
    // Start is called before the first frame update
    public int set_FPS = 30;

    public enum GameState { None, OnUI, /*OnSkip, OnAuto,*/ OnMove, OnSelf, OnPlay, OnPick }
    public GameState CurrentGameState = GameState.OnMove;
    public List<piece> piece_fab_list;
    public List<piece> cur_piece_list = new List<piece>();
    // public ObjectPool<piece> objectPool;
    public int cur_piece_index;
    public int rd_max_num;
    public int rd_min_num;
    public int white_num;
    public int black_num;
    public int score = 0;
    public piece cur_piece = null;
    public float shot_delay = 0.5f;
    public Bullet_clip bullet_Clip;
    public HP_board self_hp_board;
    public Items cur_pick_item;
    public Vector3 pick_position;
    public GameObject main_gun;
    public GameObject self_gun;
    public GameObject shatter;
    public Joker self_joker;
    public Item_pack item_pack;

// [SerializeField]
//     public Dictionary<Piece_Kind, Sprite> piece_kind_sprite_dic = new Dictionary<Piece_Kind, Sprite>();


    public void set_states(GameState new_State)
    {
        Debug.Log(" CurrentGameState = new_State " + CurrentGameState + new_State);

        // if (CurrentGameState != new_State)
        // {
        // switch (CurrentGameState)
        // {
        //     case GameState.OnMove:
        //         break;
        //     case GameState.OnPlay:
        //         break;
        //     case GameState.OnUI:
        //         break;
        // }
        CurrentGameState = new_State;
        switch (new_State)
        {
            case GameState.OnMove:
                cur_piece.gameObject.SetActive(true);
                map.Instance.boardHolder.gameObject.SetActive(true);
                main_gun.SetActive(true);
                self_gun.SetActive(false);
                break;
            case GameState.OnSelf:
                cur_piece.gameObject.SetActive(false);
                self_gun.SetActive(true);
                main_gun.SetActive(false);
                break;
            case GameState.OnPlay:
                break;
            case GameState.OnUI:
                break;
                // }
        }
    }
    public void out_states(GameState out_State)
    {

        switch (out_State)
        {
            case GameState.OnMove:
                // cur_piece.gameObject.SetActive(false);
                // map.Instance.boardHolder.gameObject.SetActive(false);
                break;
            case GameState.OnSelf:
                break;
            case GameState.OnPlay:
                break;
            case GameState.OnUI:
                break;
        }
    }
    IEnumerator delay_set(GameState new_GameState)
    {
        yield return new WaitForSeconds(0.1f); // 等待指定时间  
        set_states(new_GameState);
    }
    public void pick(Items items)
    {
        if (CurrentGameState == GameState.OnMove)
        {
            cur_pick_item = items;
            cur_piece.gameObject.SetActive(false);
            out_states(CurrentGameState);
            set_states(GameState.None);
            StartCoroutine(delay_set(GameState.OnPick));
            cur_pick_item.pick();
        }
    }
    public void clear_map()
    {
        // set_states(GameState.OnPlay);
        StartCoroutine(clear_map_func());
    }
    IEnumerator clear_map_func()
    {
        set_states(GameState.OnPlay);
        Sound_Man.Instance.PlayOther();
        yield return new WaitForSeconds(1f); // 等待指定时间  
        map.Instance.eliminate_all();
        set_states(GameState.OnMove);
    }
    public void change_one_bullet()
    {
        // set_states(GameState.OnPlay);
        StartCoroutine(change_func());
    }
    IEnumerator change_func()
    {
        set_states(GameState.OnPlay);
        Sound_Man.Instance.PlayOther();
        cur_piece.change_self();
        yield return new WaitForSeconds(1f); // 等待指定时间  
        set_states(GameState.OnMove);
    }
    public void unload_one_bullet()
    {
        // set_states(GameState.OnPlay);
        StartCoroutine(unload_func());
    }
    IEnumerator unload_func()
    {
        set_states(GameState.OnPlay);
        Sound_Man.Instance.PlayShot();
        var kind = cur_piece.piece_kind;
        GameObject.Destroy(cur_piece.gameObject);
        cur_piece = null;
        yield return new WaitForSeconds(1f); // 等待指定时间  
        bullet_Clip.show_one_bullet(kind);
        yield return new WaitForSeconds(2f); // 等待指定时间  
        if (cur_piece_index >= cur_piece_list.Count)
        {
            map.Instance.boardHolder.gameObject.SetActive(false);
            rd_gen_piece_group();
        }
        else
        {
            cur_piece = gen_piece();
            set_states(GameState.OnMove);
        }
    }
    public void reload_bullets()
    {
        // set_states(GameState.OnPlay);
        StartCoroutine(reload_func());
    }
    public void re_start(){
        self_hp_board.re_start();
        self_joker.re_start();
        UI.Instance.set_score(0);
        map.Instance.eliminate_all();
        item_pack.clear_items();
        set_states(GameState.None);  
        rd_gen_piece_group();
    }
    IEnumerator reload_func()
    {
        set_states(GameState.OnPlay);
        Sound_Man.Instance.PlayShot();
        GameObject.Destroy(cur_piece.gameObject);
        cur_piece = null;
        yield return new WaitForSeconds(1f); // 等待指定时间  
        rd_gen_piece_group();
        yield return new WaitForSeconds(2f); // 等待指定时间  
        set_states(GameState.OnMove);

    }
        void game_over_func()
    {
        Debug.Log("game over");
        StartCoroutine(over_func());
    }
        IEnumerator over_func()
    {
        if(cur_piece!=null){
            GameObject.Destroy(cur_piece);
            cur_piece = null;
        }
        set_states(GameState.None);
        // Sound_Man.Instance.PlayOther();
        self_joker.over();
        yield return new WaitForSeconds(1.5f); // 等待指定时间  
        UI.Instance.show_over();
    }
    IEnumerator use_func()
    {
        set_states(GameState.OnPlay);
        Sound_Man.Instance.PlayOther();
        yield return new WaitForSeconds(0.5f); // 等待指定时间  
        cur_pick_item.use();
    }
    public void trigger_shatter(){
        shatter.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        shatter.SetActive(true);
    }
    IEnumerator shot_self_func()
    {
        trigger_shatter();
        set_states(GameState.OnPlay);
        Sound_Man.Instance.PlayShot();
        var kind = cur_piece.piece_kind;
        GameObject.Destroy(cur_piece.gameObject);
        cur_piece = null;
        yield return new WaitForSeconds(1f); // 等待指定时间  
        self_hp_board.hurt(1, false);
        if (self_hp_board.hp == 0)
        {
            game_over_func();
            yield break;
        }
        yield return new WaitForSeconds(0.5f); // 等待指定时间  
        bullet_Clip.show_one_bullet(kind, false);
        yield return new WaitForSeconds(2f); // 等待指定时间  
        if (cur_piece_index >= cur_piece_list.Count)
        {
            map.Instance.boardHolder.gameObject.SetActive(false);
            rd_gen_piece_group();
        }
        else
        {
            cur_piece = gen_piece();
            set_states(GameState.OnMove);
        }
    }
    void rd_gen_piece_group()
    {
        int rd_num = Random.Range(rd_min_num, rd_max_num + 1);
        cur_piece_list.Clear();
        white_num = 0;
        black_num = 0;
        cur_piece_index = 0;
        for (int i = 0; i < rd_num; i++)
        {
            piece one_piece = piece_fab_list[Random.Range(0, piece_fab_list.Count)];
            cur_piece_list.Add(one_piece);
            if (one_piece.piece_kind == Piece_Kind.White)
            {
                white_num += 1;
            }
            else
            {
                black_num += 1;
            }
        }
        cur_piece = gen_piece();
        bullet_Clip.set_bullets(white_num, black_num);

    }



    IEnumerator shot_func()
    {
        // CurrentGameState = GameState.None;
        trigger_shatter();
        set_states(GameState.OnPlay);
        out_states(GameState.OnMove);
        cur_piece.set_func();
        Sound_Man.Instance.PlayOther();
        cur_piece = null;
        yield return new WaitForSeconds(shot_delay); // 等待指定时间  
        List<Vector3> list_vec = map.instance.check_eliminate();
        bool has_space = true;
        if (list_vec.Count > 0)
        {
            foreach (var vec in list_vec)
            {
                int vec_int_x = (int)vec.x;
                int vec_int_y = (int)vec.y;
                map.instance.eliminate_single(vec_int_x, vec_int_y);
            }
            score += list_vec.Count;
            UI.Instance.set_score(score);
            Sound_Man.Instance.PlayShot();
            self_joker.hurt();
            item_pack.rd_gen_item();
            // self_hp_board.hurt(1);
        }
        else
        {
            has_space = map.instance.check_has_space();
        }
        if (!has_space)
        {
            game_over_func();
            yield break;
        }
        if (cur_piece_index >= cur_piece_list.Count)
        {
            map.Instance.boardHolder.gameObject.SetActive(false);
            Debug.Log(" out");
            rd_gen_piece_group();
        }
        else
        {
            cur_piece = gen_piece();
            // CurrentGameState = GameState.OnMove;
            set_states(GameState.OnMove);
        }
    }

    piece gen_piece()
    {
        piece piece_fab = cur_piece_list[cur_piece_index];
        UI.Instance.set_bullet_num(cur_piece_list.Count - cur_piece_index);
        cur_piece_index++;
        piece instance = Instantiate(piece_fab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<piece>() as piece;
        instance.transform.SetParent(map.Instance.boardHolder);
        instance.state_to_moving();
        instance.move_by_mouse();

        return instance;
    }

    void Start()
    {
        // Application.targetFrameRate=set_FPS;
        set_states(GameState.None);  
        map.Instance.boardHolder.gameObject.SetActive(false);
        rd_gen_piece_group();
        // cur_piece = gen_piece();

    }
    void OnDestroy()
    {
        cur_piece_list.Clear();
    }
    public void check_mouse_item()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, -1);

        // 确定射线的方向，这里假设是物体正前方  
        // Vector3 direction = transform.forward;
        Vector3 direction = new Vector3(0, 0, 1);
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);
        if (hits.Length != 0)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Items"))
                {
                    Debug.Log("hit.collider.CompareTag : items " + hit.collider.name);
                    pick(hit.collider.GetComponent<Items>());
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

        if (CurrentGameState == GameState.OnMove)
        {
            // if (Input.GetMouseButtonDown(1))
            // {
            //     cur_piece = gen_piece();
            // }
            if (cur_piece != null && cur_piece.piece_state == Piece_State.moving)
            {

                cur_piece.move_by_mouse();
                if (Input.GetMouseButtonDown(0))
                {
                    if (cur_piece.build_able)
                    {
                        // shot_func();
                        StartCoroutine(shot_func());

                    }
                    else
                    {
                        Debug.Log("can't build");
                        check_mouse_item();
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    out_states(CurrentGameState);
                    set_states(GameState.None);
                    StartCoroutine(delay_set(GameState.OnSelf));
                }
            }
        }
        if (CurrentGameState == GameState.OnPick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(use_func());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                cur_pick_item.unpick();
                out_states(CurrentGameState);
                set_states(GameState.OnMove);
            }
        }
        if (CurrentGameState == GameState.OnSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(shot_self_func());
            }
            else if (Input.GetMouseButtonDown(1))
            {

                out_states(CurrentGameState);
                set_states(GameState.OnMove);
            }
        }

    }
}
