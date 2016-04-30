using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using JBirdEngine.EditorHelper;
#endif
using JBirdEngine;

[System.Serializable]
public class JBirdRopeNode : MonoBehaviour {

    #if UNITY_EDITOR
    [ViewOnly]
    #endif
    [SerializeField]
	private JBirdRope _rope;
	public JBirdRope rope {
		get {
			return _rope;
		}
		set {
			if (_rope == null) {
				_rope = value;
			}
		}
	}

    #if UNITY_EDITOR
    [ViewOnly]
    #endif
    [SerializeField]
	private JBirdRopeNode _prev;
	public JBirdRopeNode prev {
		get {
			return _prev;
		}
		set {
			if (_prev == null) {
				_prev = value;
			}
			else if (value == null) {
				_prev = value;
			}
		}
	}

    #if UNITY_EDITOR
    [ViewOnly]
    #endif
    [SerializeField]
	private JBirdRopeNode _next;
	public JBirdRopeNode next {
		get {
			return _next;
		}
		set {
			if (_next == null) {
				_next = value;
			}
			else if (value == null) {
				_next = value;
			}
		}
	}

	public bool isAnchor;
	public int index;
	public float scale;
	public Renderer rend;

	void Awake () {
		rend = GetComponent<Renderer>();
	}

	void Start () {
				
	}

	void Update () {
				
	}

	public void Pull (Vector3 direction, float magnitude) {
		PullRaw(direction, magnitude, false);
	}

	public void Pull (Vector3 position) {
		PullRaw(position, false);
	}

	public void PullRaw (Vector3 direction, float magnitude, bool ignoreAnchors = true) {
		PullRaw(transform.position + direction * magnitude);
	}

	public void PullRaw (Vector3 position, bool ignoreAnchors = true) {
		_rope.iterations = 0;
		if (!ignoreAnchors && isAnchor) {
			return;
		}
		transform.position = position;
		PercolatePrev(ignoreAnchors);
		PercolateNext(ignoreAnchors);
	}

	void PercolatePrev (bool ignoreAnchors) {
		if (_prev == null) {
			return;
		}
		_rope.iterations++;
		if (_rope.iterations >= _rope.iterationLimit) {
			return;
		}
		if (!ignoreAnchors && _prev.isAnchor) {
			_prev.PercolateNext(ignoreAnchors);
		}
		else {
			BringTowardsThis(_prev);
			_prev.PercolatePrev(ignoreAnchors);
		}
	}

	void PercolateNext (bool ignoreAnchors) {
		if (_next == null) {
			return;
		}
		_rope.iterations++;
		if (_rope.iterations >= _rope.iterationLimit) {
			return;
		}
		if (!ignoreAnchors && _next.isAnchor) {
			_next.PercolatePrev(ignoreAnchors);
		}
		else {
			BringTowardsThis(_next);
			_next.PercolateNext(ignoreAnchors);
		}
	}

	void BringTowardsThis (JBirdRopeNode other) {
		float dist = Vector3.Distance(transform.position, other.transform.position);
		float newDist = dist;
		if (dist < _rope.segmentLength * _rope.minTension * (scale + other.scale) / 2f) {
			newDist = _rope.segmentLength * _rope.minTension * (scale + other.scale) / 2f;
		}
		else if (dist > _rope.segmentLength * _rope.maxTension * (scale + other.scale) / 2f) {
			newDist = _rope.segmentLength * _rope.maxTension * (scale + other.scale) / 2f;
		}
		if (newDist == dist) {
			return;
		}
		other.transform.position = transform.position + (other.transform.position - transform.position).normalized * newDist;
	}

	public void UpdateRopeHead () {
		Vector3 newHeadPos = _rope.segmentList[0].transform.position;
		Vector3 newHeadForward = _rope.segmentList[0].transform.forward;
		Vector3 newHeadUp = _rope.segmentList[0].transform.up;
		Vector3 localOffset = _rope.transform.position - newHeadPos;
		for (int i = 0; i < _rope.segmentList.Count; i++) {
			_rope.segmentList[i].transform.position += localOffset;
			_rope.segmentList[i].transform.parent = null;
		}
		_rope.transform.rotation = Quaternion.LookRotation(newHeadForward, newHeadUp);
		for (int i = 0; i < _rope.segmentList.Count; i++) {
			_rope.segmentList[i].transform.parent = _rope.transform;
		}
		_rope.transform.position = newHeadPos;
	}

	public void Reorient () {
		Vector3 newForward = Vector3.zero;
		if (_prev != null) {
			newForward += _prev.transform.position - transform.position;
		}
		if (_next != null) {
			newForward += transform.position - _next.transform.position;
		}
		if (newForward == Vector3.zero) {
			return;
		}
		newForward.Normalize();
		Vector3 newUp = Vector3.Cross(newForward, transform.right);
		if (_prev != null) {
			newUp += Vector3.Cross(_prev.transform.forward, _prev.transform.right);
			newUp.Normalize();
		}
		transform.rotation = Quaternion.LookRotation(newForward, newUp);
	}

}