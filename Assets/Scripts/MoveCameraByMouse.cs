using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;


namespace KDSP
{
    /// <summary>
    /// 修改记录：史苏洋 2022/4/11 增加相机自动旋转方法
    /// </summary>
    public class MoveCameraByMouse : MonoBehaviour
    {
        public static MoveCameraByMouse mainGodCam;
        public Transform target;
        public float x;//x轴初始角度
        public float y;//y轴初始角度
        public float xSpeed = 10;//x轴转速
        public float ySpeed = 10;//y轴转速
        public float mSpeed = 10;//滚轴速度
        public float aimMoveSpeed = 10f;//中键移动速度
        public float yMinLimit = 10;//y轴最小角度
        public float yMaxLimit = 90;//y轴最大角度
        public float distance = 100;//变换后的距离
        public float minDistance = 1;//最小距离
        public float maxDistance = 4500; //最大距离
        public bool needDamping = true;//是否有阻力
        public float damping = 3.0f;//阻力值（越小越平滑）
        float m_distance;//当前的距离
        public float posx;
        public float posy;
        Quaternion m_rotation;//当前的角度
        Quaternion rotation;//变换后的角度
        Vector3 position;//变换后的位置
        Vector3 m_position;//当前的位置
        public bool IsLeftControl;
        public bool MoveEnabled = true;//是否允许移动
        public bool RotateEnabled = true;//是否允许旋转
        public bool WheelZoomEnabled = true;//是否允许滚轮缩放
        private bool IsOrthogonality = false;//是否为正交视角
        float orthographicSize = 250;
        float m_orthographicSize;
        Coroutine moveCam;
        public bool autoRoat;
        public float autotime;
        public float direction = 1;
        public float AngleY;
        public bool isLock = false;

        public double lat;
        public double lon;
        /// <summary>
        /// 相机级别
        /// </summary>
        public int cameraLevel = 1;


        void Start()
        {
            Debug.Log(Application.persistentDataPath);
            //m_distance = (transform.position - target.position).magnitude;//获取摄像机初始距离
            canvas=this.GetComponentInChildren<Canvas>();
            //Invoke("stopain",11);
        }
        private void OnEnable()
        {
            mainGodCam = this;
        }
        void stopain()
        {
            //GetComponent<Animator>().enabled = false;
            needDamping = true;
        }
        // Update is called once per frame  
        void Update()
        {
            if (Mouse.current == null || Keyboard.current == null)
            {
                return;
            }
            if (!isLock)
            {
                MouseRotate();
                MouseMove();
                MouseWheelZoom();
            }


            if (this.transform.position.y < 1000)
            {
                cameraLevel = 18;
            }
            else if (this.transform.position.y < 2000)
            {
                cameraLevel = 17;
            }
            else if (this.transform.position.y < 3000)
            {
                cameraLevel = 16;
            }
            else if (this.transform.position.y < 4000)
            {
                cameraLevel = 15;
            }
            else if (this.transform.position.y < 5000)
            {
                cameraLevel = 9;
            }


            //if (this.transform.position.y < 200)
            //{
            //    cameraLevel = 18;
            //}
            //else if (this.transform.position.y < 400)
            //{
            //    cameraLevel = 17;
            //}
            //else if (this.transform.position.y < 600)
            //{
            //    cameraLevel = 16;
            //}
            //else if (this.transform.position.y < 800)
            //{
            //    cameraLevel = 15;
            //}
            //else if (this.transform.position.y < 1000)
            //{
            //    cameraLe vel = 9;
            //}

        }
        Canvas canvas;
        //private void LateUpdate()
        //{
        //    GetComponent<Camera>().nearClipPlane = Mathf.Clamp(GetComponent<Camera>().nearClipPlane = distance / 1000f, 0.3f, 1000f);
        //    //canvas.planeDistance = GetComponent<Camera>().nearClipPlane + 0.1f;//----赵哥改6.9号
        //}
        /// <summary>
        /// 摄像机平移
        /// </summary>
        /// <param name="mouse"></param>
        public void MouseMove()
        {
            if (!MoveEnabled)
                return;
            if (Mouse.current.middleButton.wasReleasedThisFrame)
            {
            }
            if (Mouse.current.middleButton.isPressed)
            {
                Vector2 v2 = Mouse.current.position.ReadValue();
                if (posx == 0 && posy == 0)
                {
                    posx = v2.x;
                    posy = v2.y;
                }
                float newx = v2.x;
                float newy = v2.y;
                float mx = newx - posx;
                float my = newy - posy;
                target.localEulerAngles = new Vector3(0, m_rotation.eulerAngles.y, 0);


                float m_dis = distance;
                //  m_dis = Mathf.Clamp(m_dis, 1f, 10000f);
                if (m_dis < 10) m_dis = 10;
                target.Translate(-mx * aimMoveSpeed * m_dis / 5000, 0, -my * aimMoveSpeed * m_dis / 5000, Space.Self);
                posx = v2.x;
                posy = v2.y;
            }
            else
            {
                posx = 0;
                posy = 0;
            }
            //if (Keyboard.current.wKey.isPressed)
            //{
            //    target.localEulerAngles = new Vector3(0, m_rotation.eulerAngles.y, 0);
            //    target.Translate(0, 0, aimMoveSpeed * distance * Time.deltaTime, Space.Self);
            //}
            //if (Keyboard.current.sKey.isPressed)
            //{
            //    target.localEulerAngles = new Vector3(0, m_rotation.eulerAngles.y, 0);
            //    target.Translate(0, 0, -aimMoveSpeed * distance * Time.deltaTime, Space.Self);
            //}
            //if (Keyboard.current.aKey.isPressed)
            //{
            //    target.localEulerAngles = new Vector3(0, m_rotation.eulerAngles.y, 0);
            //    target.Translate(-aimMoveSpeed * distance * Time.deltaTime, 0, 0, Space.Self);
            //}
            //if (Keyboard.current.dKey.isPressed)
            //{
            //    target.localEulerAngles = new Vector3(0, m_rotation.eulerAngles.y, 0);
            //    target.Translate(aimMoveSpeed * distance * Time.deltaTime, 0, 0, Space.Self);
            //}

        }

        /// <summary>
        /// 摄像机镜头旋转
        /// </summary>
        public void MouseRotate()
        {
            if (!RotateEnabled)
                return;
            if (target)
            {
                //use the light button of mouse to rotate the camera  
                if (Mouse.current.leftButton.isPressed)
                {
                    Vector2 v2 = Mouse.current.delta.ReadValue();
                    x += v2.x * xSpeed * 0.02f;
                    y -= v2.y * ySpeed * 0.02f;
                }
                else
                {
                    if (autoRoat)
                    {
                        //x = x + Time.deltaTime * 5;
                        x = x + (360 / autotime) * Time.deltaTime * direction;
                        //  y = AngleY;
                    }
                }
                //if (Keyboard.current.upArrowKey.isPressed)
                //{
                //    y += 500 * ySpeed * Time.deltaTime * 0.02f;
                //}
                //if (Keyboard.current.downArrowKey.isPressed)
                //{
                //    y -= 500 * ySpeed * Time.deltaTime * 0.02f;
                //}
                //if (Keyboard.current.leftArrowKey.isPressed)
                //{
                //    x += 500 * xSpeed * Time.deltaTime * 0.02f;
                //}
                //if (Keyboard.current.rightArrowKey.isPressed)
                //{
                //    x -= 500 * xSpeed * Time.deltaTime * 0.02f;
                //}
                while (x < -180)
                {
                    x = x + 360;
                }
                while (x > 180)
                {
                    x = x - 360;
                }
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                rotation = Quaternion.Euler(y, x, 0);
                m_rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                position = target.position - rotation * Vector3.forward * distance;
                m_position = target.position - m_rotation * Vector3.forward * m_distance;
                if (needDamping)
                {
                    transform.rotation = m_rotation;
                    transform.position = m_position;
                }
                else
                {
                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }

        /// <summary>
        /// 鼠标滚轮缩放
        /// </summary>
        public void MouseWheelZoom()
        {
            if (!WheelZoomEnabled)
                return;
            if (target)
            {
                if (IsOrthogonality)
                {
                    orthographicSize += Mouse.current.scroll.ReadValue().y * mSpeed;
                    orthographicSize = Mathf.Clamp(orthographicSize, minDistance, maxDistance);
                    m_orthographicSize = Mathf.Lerp(m_orthographicSize, orthographicSize, Time.deltaTime * damping);
                    if (needDamping)
                    {
                        this.GetComponent<Camera>().orthographicSize = m_orthographicSize;
                    }
                    else
                    {
                        this.GetComponent<Camera>().orthographicSize = orthographicSize;
                    }
                }
                else
                {
                    float m_dis = distance;
                    if (m_dis < 10) m_dis = 10;
                    distance -= Mouse.current.scroll.ReadValue().y * mSpeed * m_dis / 10000;
                    //限制distance最小值
                    distance = Mathf.Clamp(distance, minDistance, maxDistance);
                    position = target.position - rotation * Vector3.forward * distance;
                    m_distance = Mathf.Lerp(m_distance, distance, Time.deltaTime * damping);

                    m_position = target.position - m_rotation * Vector3.forward * m_distance;

                    //adjust the camera  
                    if (needDamping)
                    {
                        transform.position = m_position;
                    }
                    else
                    {
                        transform.position = position;
                    }
                }
            }
        }


        public void SetCamera(Vector3 v3, float distance, float roatx, float roaty)
        {
            target.DOMove(v3, 0.5f);
            this.distance = distance;
            this.x = roatx;
            this.y = roaty;
        }
        /// <summary>
        /// 切换摄像机为正交或透视
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="IsOrthogonality"></param>
        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
        public void SetRot(float speed)
        {
            x += speed * 0.02f;
            rotation = Quaternion.Euler(y, x, 0);
        }
        public void Reset(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public void SetCamera(float duration, float x, float y, float z, float distance, float roatx, float roaty)
        {
            if (moveCam != null)
            {
                StopCoroutine(moveCam);
            }
            if (duration <= 0)
            {
                target.position = new Vector3(x, y, z);
                this.distance = distance;
                this.x = roatx;
                this.y = roaty;
            }
            else
            {
                moveCam = StartCoroutine(camMove(distance, roatx, roaty, duration, new Vector3(x, y, z)));
            }
        }
        IEnumerator camMove(float distance, float roatx, float roaty, float duration, Vector3 v3)
        {
            needDamping = false;
            float time = 0;
            while (x < -180)
            {
                x = x + 360;
            }
            while (x > 180)
            {
                x = x - 360;
            }
            float oldx = this.x;
            float oldy = this.y;
            while (roatx < -180)
            {
                roatx = roatx + 360;
            }
            while (roatx > 180)
            {
                roatx = roatx - 360;
            }
            if (Mathf.Abs(oldx - roatx) > 180)
            {
                if (oldx > roatx)
                {
                    oldx = oldx - 180;
                }
                else
                {
                    roatx = roatx - 180;
                }
            }
            float olddistance = this.distance;
            Vector3 oldv3 = this.target.position;
            while (time < duration)
            {
                time = time + Time.deltaTime;
                this.x = Mathf.Lerp(oldx, roatx, time / duration);
                this.y = Mathf.Lerp(oldy, roaty, time / duration);
                this.distance = Mathf.Lerp(olddistance, distance, time / duration);
                target.position = Vector3.Lerp(oldv3, v3, time / duration);
                m_distance = distance;
                m_rotation = transform.rotation;
                m_position = transform.position;
                m_orthographicSize = this.GetComponent<Camera>().orthographicSize;
                target.position = Vector3.Lerp(oldv3, v3, time / duration);
                yield return new WaitForEndOfFrame();
            }
            needDamping = true;
        }

    }
}
