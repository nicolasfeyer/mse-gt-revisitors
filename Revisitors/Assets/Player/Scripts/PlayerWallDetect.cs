using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallDetect : MonoBehaviour {
    [SerializeField] ContactFilter2D groundFilter;
    [SerializeField] private float minLeftAngle;
    [SerializeField] private float maxLeftAngle;
    [SerializeField] private float minRightAngle;
    [SerializeField] private float maxRightAngle;

    private Rigidbody2D rb2d;
    private PlayerCtrl ctrl;
    private bool wasGoingRight = true;

    // Start is called before the first frame update
    void Awake() {
        ctrl = GetComponent<PlayerCtrl>();
        rb2d = GetComponent<Rigidbody2D>();

        groundFilter.minNormalAngle = minRightAngle;
        groundFilter.maxNormalAngle = maxRightAngle;
    }

    // Update is called once per frame
    void Update() {
        CheckDirection();
        ctrl.IsAgainstWall = rb2d.IsTouching(groundFilter);
    }

    private void CheckDirection() {
        // Changed direction
        if(ctrl.IsGoingRight != wasGoingRight) {
            groundFilter.minNormalAngle = ctrl.IsGoingRight ? minRightAngle : minLeftAngle;
            groundFilter.maxNormalAngle = ctrl.IsGoingRight ? maxRightAngle : maxLeftAngle;
            wasGoingRight = ctrl.IsGoingRight;
        }
    }
}
