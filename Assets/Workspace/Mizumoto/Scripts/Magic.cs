using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region コンポーネントのアタッチ
[RequireComponent(typeof(Rigidbody))]
#endregion

// 実際に攻撃判定を持つオブジェクト
public class Magic : MagicBase
{
    private void OnTriggerEnter(Collider other)
    {
        // ダメージを与える
        // OnTriggerEnterなのでよほどのことがない限り二回受けることはないと思う
        // ヒットしたオブジェクトの情報を取得して同じオブジェクトには当たらないみたいなものも
        // 作れるが計算コストが上がるので今はやめておく 困ったら実装する
    }
}
