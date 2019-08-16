using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using VRCSDK2;
using System.Threading;

public class TKCH_AvatarSetup
{
	[MenuItem("CONTEXT/VRC_AvatarDescriptor/VisemeRipSyncSetup")]
	private static void visemeRipSyncSetup(MenuCommand command)
	{
		if (!EditorUtility.DisplayDialog (
			"Custom Script Message", "Lip Sync を Viseme に自動設定します。続行しますか？", 
			"続行", "cancel")) {
			return;
		}

		VRC_AvatarDescriptor vrcAd = (VRC_AvatarDescriptor)command.context;

		GameObject goAvatar = vrcAd.gameObject;

		// あらかじめViseme名リストを作成しておく
		string[] vNameList = new string[(int)VRC_AvatarDescriptor.Viseme.Count];
		for (int i = 0; i < (int)VRC_AvatarDescriptor.Viseme.Count; i++) {
			vNameList[i] = ((VRC_AvatarDescriptor.Viseme)i).ToString();
		}

		SkinnedMeshRenderer targetSmr = null;

		// オブジェクト指定済み
		if (vrcAd.VisemeSkinnedMesh != null) {
			
			if (hasVisemeBlendShapes (vrcAd.VisemeSkinnedMesh.sharedMesh, vNameList)) {
				if (!EditorUtility.DisplayDialog (
					"Custom Script Message", "オブジェクトがすでに設定されています。続行しますか？", 
					"続行", "cancel")) {
					return;
				}
				targetSmr = vrcAd.VisemeSkinnedMesh;
			} else {
				EditorUtility.DisplayDialog ("Custom Script Message", "オブジェクトがすでに設定されています。", "Close");
				return;
			}

		} else { // オブジェクトが未指定
			
			// 優先して設定するオブジェクトの名前
			string[] firstSearchMeshNames = new string[] {
				"face",
				"body"
			};

			foreach(string name in firstSearchMeshNames)
			{
				Transform tr = goAvatar.transform.Find (name);
				if (tr == null) {
					continue;
				}				
				SkinnedMeshRenderer smr = tr.GetComponent<SkinnedMeshRenderer> ();
				if (hasVisemeBlendShapes (smr.sharedMesh, vNameList)) {
					targetSmr = smr;
					break;
				}
			}

			// 見つからない場合、全てのオブジェクトにVisemeのBlendShape名を持つものがないか検索する
			if (targetSmr == null) {
				foreach (SkinnedMeshRenderer smr in goAvatar.transform.GetComponentsInChildren<SkinnedMeshRenderer> ()) {
					// 検索済みのオブジェクトは除外
					if (0 <= Array.FindIndex<string> (firstSearchMeshNames, delegate (string s) { return s == smr.name; })) {
						continue;
					}
					if (hasVisemeBlendShapes (smr.sharedMesh, vNameList)) {
						targetSmr = smr;
						break;
					}
				}
			}

			if (targetSmr == null) {
				EditorUtility.DisplayDialog ("Custom Script Message", "リップシンク対象のオブジェクトが見つかりませんでした。", "Close");
				return;
			}
		}

		vrcAd.lipSync = VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape;
		vrcAd.VisemeSkinnedMesh = targetSmr;
		if (vrcAd.VisemeBlendShapes == null || vrcAd.VisemeBlendShapes.Length == 0) 
		{
			vrcAd.VisemeBlendShapes = new string[(int)VRC_AvatarDescriptor.Viseme.Count];
		}
		Mesh sm = targetSmr.sharedMesh;
		for(int i = 0; i < vNameList.Length; i++)
		{
			if(vrcAd.VisemeBlendShapes.Length <= i)
			{
				break;
			}
			string vName = vNameList[i];
			for(int j = 0; j < sm.blendShapeCount; j++)
			{
				string bName = sm.GetBlendShapeName (j);
				// 先頭または末尾がViseme名に一致しているかどうか
				if (bName.StartsWith (vName) || bName.EndsWith (vName)) 
				{
					vrcAd.VisemeBlendShapes[ i ] = bName;
					break;
				}
			}
		}
	}

	/// <summary>
	/// Hases the viseme blend shapes.
	/// </summary>
	/// <returns><c>true</c>, if viseme blend shapes was hased, <c>false</c> otherwise.</returns>
	/// <param name="m">M.</param>
	/// <param name="Names">Names.</param>
	private static bool hasVisemeBlendShapes(Mesh m, string[] Names) {
		
		if(m.blendShapeCount < Names.Length) {
			return false;
		}

		bool vNameMissing = false;
		foreach (string vName in Names) {
			bool vNameFound = false;
			for (int i = 0; i < m.blendShapeCount; i++) {
				string bName = m.GetBlendShapeName (i);
				// 先頭または末尾がViseme名に一致しているかどうか
				if (bName.StartsWith (vName) || bName.EndsWith (vName)) 
				{
					vNameFound = true;
					break;
				}
			}
			if (!vNameFound) {
				vNameMissing = true;
				break;
			}
		}
		if(vNameMissing) {
			return false;
		}

		return true;
	}
}
