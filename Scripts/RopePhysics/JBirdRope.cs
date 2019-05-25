//Comment out the following line if you aren't using the JBird Color Library
#define COLOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdLib.EditorHelper;
using JBirdLib.ColorLibrary;

public class JBirdRope : MonoBehaviour {
	[Header("Physical Attributes:")]
	public float segmentLength;
	public int numberOfSegments;
	[Range(0f,1f)]
	public float elasticity = 0.5f;
    [ViewOnly]
    [SerializeField]
	private float _minTension;
    [ViewOnly]
    [SerializeField]
	private float _maxTension;
	[Range(0.001f, 1f)]
	public float tapering = 1f;
	[Header("Anchoring:")]
	public bool anchoredStart;
	public bool anchoredEnd;
	[Header("Rope Segements:")]
	public JBirdRopeNode segmentPrefab;
    [ViewOnly]
	public List<JBirdRopeNode> segmentList;
	[Header("Iteration Limit (WARNING: MODIFY AT OWN RISK):")]
	public int iterationLimit = 500;
	[HideInInspector]
	public int iterations = 0;
	private bool _updateTension = true;
	[Header("Rope Options:")]
	public bool equalizeTension;
	public bool reorientSegments;
	public bool pullFromHead;
	private Vector3 lastPos;
	[HideInInspector]
	public Color c1;
	[HideInInspector]
	public Color c2;
	[HideInInspector]
	public bool useGradient;
	private bool _useGradient;

	public float minTension {
		get {
			if (_updateTension) {
				UpdateTensionVars();
			}
			return _minTension;
		}
	}

	public float maxTension {
		get {
			if (_updateTension) {
				UpdateTensionVars();
			}
			return _maxTension;
		}
	}

	void Awake () {
		_updateTension = true;
		lastPos = transform.position;
		if (useGradient) {
			MakeGradient();
		}
		_useGradient = useGradient;
	}

	void Update () {
		UpdateTapering();
		if (segmentList.Count > 0 && pullFromHead) {
			for (int i = 0; i < segmentList.Count; i++) {
				segmentList[i].transform.position -= transform.position - lastPos;
			}
			segmentList[0].Pull(transform.position);
		}
		if (equalizeTension) {
			EqualizeTension();
		}
		if (reorientSegments) {
			ReorientSegments();
		}
		if (segmentList.Count > 0) {
			segmentList[0].UpdateRopeHead();
		}
		lastPos = transform.position;
		#if COLOR
		if (useGradient && !_useGradient) {
			MakeGradient();
			_useGradient = useGradient;
		}
		else if (!useGradient && _useGradient) {
			ClearColor();
			_useGradient = useGradient;
		}
		#endif
	}

	public void UpdateTensionVars () {
		_maxTension = 1f;
		_minTension = _maxTension - elasticity;
	}

	public void UpdateAnchors () {
		if (segmentList.Count == 0) {
			return;
		}
		segmentList[0].isAnchor = anchoredStart;
		segmentList[segmentList.Count - 1].isAnchor = anchoredEnd;
	}

	public void CreateRope (Vector3 direction, float tension) {
		if (segmentPrefab == null) {
			Debug.LogError("JBirdRope.CreateRope(): Segment prefab undefined.");
			return;
		}
		if (segmentList.Count > 0) {
			Resize(direction, tension);
			return;
		}
		segmentList = new List<JBirdRopeNode>();
		for (int i = 0; i < numberOfSegments; i++) {
			MakeNewSegment(transform.position + direction * (float)i * tension * segmentLength);
		}
		UpdateTapering();
	}

	public void Resize (Vector3 direction, float tension) {
		while (segmentList.Count > numberOfSegments) {
			JBirdRopeNode node = segmentList[segmentList.Count - 1];
			segmentList.RemoveAt(segmentList.Count - 1);
			if (Application.isPlaying) {
				Destroy(node.gameObject);
			}
			else {
				DestroyImmediate(node.gameObject);
			}
		}
		segmentList[segmentList.Count - 1].next = null;
		for (int i = segmentList.Count - 1; i < numberOfSegments - 1; i++) {
			MakeNewSegment(segmentList[i].transform.position + direction * tension * segmentLength);
		}
		for (int i = 0; i < segmentList.Count; i++) {
			segmentList[i].isAnchor = false;
		}
		UpdateAnchors();
		UpdateTapering();
	}

	JBirdRopeNode MakeNewSegment (Vector3 position) {
		JBirdRopeNode newNode = Instantiate(segmentPrefab, position, Quaternion.identity) as JBirdRopeNode;
		segmentList.Add(newNode);
		newNode.transform.parent = transform;
		newNode.rope = this;
		newNode.index = segmentList.Count - 1;
		newNode.rend = newNode.GetComponent<Renderer>();
		if (segmentList.Count > 1) {
			segmentList[segmentList.Count - 2].next = segmentList[segmentList.Count - 1];
			segmentList[segmentList.Count - 1].prev = segmentList[segmentList.Count - 2];
		}
		return newNode;
	}

	public void ReorientSegments () {
		for (int i = 0; i < segmentList.Count; i++) {
			segmentList[i].Reorient();
		}
	}

	public void EqualizeTension () {
		_updateTension = false;
		_minTension = _maxTension = GetAverageTension();
		segmentList[0].Pull(segmentList[0].transform.position);
		_updateTension = true;
		UpdateTensionVars();
	}

	public float GetAverageTension () {
		if (segmentList.Count == 0) {
			return -1f;
		}
		float avgTension = 0f;
		for (int i = 1; i < segmentList.Count; i++) {
			avgTension += Vector3.Distance(segmentList[i - 1].transform.position, segmentList[i].transform.position) / segmentLength  / (segmentList[i - 1].scale + segmentList[i].scale) * 2f;
		}
		avgTension = avgTension / (segmentList.Count - 1);
		return avgTension;
	}

	public void UpdateTapering () {
		for (int i = 0; i < segmentList.Count; i++) {
			segmentList[i].index = i;
			segmentList[i].scale = 1f - Mathf.Sqrt(1f - Mathf.Sqrt(Mathf.Sqrt(tapering))) * (1f - (float)(segmentList.Count - i) / (float)segmentList.Count);
			segmentList[i].transform.localScale = Vector3.one * segmentList[i].scale;
		}
	}

	#if COLOR
	public void ClearColor () {
		if (useGradient) {
			return;
		}
		if (segmentPrefab.rend == null) {
			Debug.LogError("JBirdRope.ClearColor(): Segment Prefab's 'rend' field is empty!");
			return;
		}
		for (int i = 0; i < segmentList.Count; i++) {
			segmentList[i].rend.material = segmentPrefab.rend.sharedMaterial;
		}
	}

	public void MakeGradient () {
		if (!useGradient) {
			return;
		}
		List<Color> gradient = ColorHelper.MakeGradientHSV(c1, c2, segmentList.Count - 2);
		for (int i = 0; i < segmentList.Count; i++) {
			segmentList[i].rend.material.color = gradient[i];
		}
	}
	#endif

}
