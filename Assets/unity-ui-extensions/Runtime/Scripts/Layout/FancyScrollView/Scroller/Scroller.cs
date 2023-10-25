/// Credit setchi (https://github.com/setchi)
/// Sourced from - https://github.com/setchi/FancyScrollView

using System;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions.EasingCore;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using UnityEngine.Serialization;

namespace UnityEngine.UI.Extensions
{

    public class Scroller : UIBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
    {
        private float maxX = 1f;
        
        private float viewCount = 1f; //보여지는 셀의 수

        [SerializeField] RectTransform viewport = default;

        public float ViewportSize => scrollDirection == ScrollDirection.Horizontal
            ? viewport.rect.size.x
            : viewport.rect.size.y;

        [SerializeField] ScrollDirection scrollDirection = ScrollDirection.Vertical;

        public ScrollDirection ScrollDirection => scrollDirection;

        [SerializeField] MovementType movementType = MovementType.Elastic;
        public MovementType MovementType
        {
            get => movementType;
            set => movementType = value;
        }

        [SerializeField] float elasticity = 0.1f;

        public float Elasticity
        {
            get => elasticity;
            set => elasticity = value;
        }

        [SerializeField] float scrollSensitivity = 1f;

        public float ScrollSensitivity
        {
            get => scrollSensitivity;
            set => scrollSensitivity = value;
        }

        [SerializeField] bool inertia = true;

        public bool Inertia
        {
            get => inertia;
            set => inertia = value;
        }

        [SerializeField] float decelerationRate = 0.03f;

        public float DecelerationRate
        {
            get => decelerationRate;
            set => decelerationRate = value;
        }

        [SerializeField] Snap snap = new Snap {
            Enable = true,
            VelocityThreshold = 0.5f,
            Duration = 0.3f,
            Easing = Ease.InOutCubic
        };

        public bool SnapEnabled
        {
            get => snap.Enable;
            set => snap.Enable = value;
        }

        [SerializeField] bool draggable = true;

        public bool Draggable
        {
            get => draggable;
            set => draggable = value;
        }

        [FormerlySerializedAs("passType")] [SerializeField] TypeScroll type = TypeScroll.None;
        [SerializeField] Scrollbar scrollbar = default;

        public Scrollbar Scrollbar => scrollbar;

        public float Position
        {
            get => currentPosition;
            set
            {
                autoScrollState.Reset();
                velocity = 0f;
                dragging = false;

                UpdatePosition(value);
            }
        }

        readonly AutoScrollState autoScrollState = new AutoScrollState();

        Action<float> onValueChanged;
        Action<int> onSelectionChanged;

        Vector2 beginDragPointerPosition;
        float scrollStartPosition;
        float prevPosition;
        float currentPosition;

        int totalCount;

        bool hold;
        bool scrolling;
        bool dragging;
        float velocity;

        [Serializable]
        class Snap
        {
            public bool Enable;
            public float VelocityThreshold;
            public float Duration;
            public Ease Easing;
        }

        static readonly EasingFunction DefaultEasingFunction = Easing.Get(Ease.OutCubic);

        class AutoScrollState
        {
            public bool Enable;
            public bool Elastic;
            public float Duration;
            public EasingFunction EasingFunction;
            public float StartTime;
            public float EndPosition;

            public Action OnComplete;

            public void Reset()
            {
                Enable = false;
                Elastic = false;
                Duration = 0f;
                StartTime = 0f;
                EasingFunction = DefaultEasingFunction;
                EndPosition = 0f;
                OnComplete = null;
            }

            public void Complete()
            {
                OnComplete?.Invoke();
                Reset();
            }
        }

        private bool _isInitailze = false;
        protected override void Start()
        {
            base.Start();

            
            if (scrollbar)
            {
                scrollbar.onValueChanged.AddListener(x => UpdatePosition(Mathf.Min(x,maxX) * (totalCount - 1f), false));
                Initialize();
                _isInitailze = true;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_isInitailze)
            {
                Initialize();
            }
        }
        public List<int> GetDolPassSplitData(string key)
        {
            List<int> returnValues = new List<int>();

            var splits = ServerData.seolPassServerTable.TableDatas[key].Value.Split(',');

            for (int i = 0; i < splits.Length; i++)
            {
                if (int.TryParse(splits[i], out var result))
                {
                    returnValues.Add(result);
                }
            }

            return returnValues;
        }
        public List<int> GetMonthPassSplitData(string key)
        {
            List<int> returnValues = new List<int>();

            var splits = ServerData.monthlyPassServerTable.TableDatas[key].Value.Split(',');

            for (int i = 0; i < splits.Length; i++)
            {
                if (int.TryParse(splits[i], out var result))
                {
                    returnValues.Add(result);
                }
            }

            return returnValues;
        }
        public List<int> GetMonthPass2SplitData(string key)
        {
            List<int> returnValues = new List<int>();

            var splits = ServerData.monthlyPassServerTable2.TableDatas[key].Value.Split(',');

            for (int i = 0; i < splits.Length; i++)
            {
                if (int.TryParse(splits[i], out var result))
                {
                    returnValues.Add(result);
                }
            }

            return returnValues;
        }
        public List<int> GetSnowAttenPassSplitData(string key)
        {
            List<int> returnValues = new List<int>();

            var splits = ServerData.oneYearPassServerTable.TableDatas[key].Value.Split(',');

            for (int i = 0; i < splits.Length; i++)
            {
                if (int.TryParse(splits[i], out var result))
                {
                    returnValues.Add(result);
                }
            }

            return returnValues;
        }
        
        public List<int> GetSuhoPassSplitData(string key)
        {
            List<int> returnValues = new List<int>();

            var splits = ServerData.coldSeasonPassServerTable.TableDatas[key].Value.Split(',');

            for (int i = 0; i < splits.Length; i++)
            {
                if (int.TryParse(splits[i], out var result))
                {
                    returnValues.Add(result);
                }
            }

            return returnValues;
        }
        
        public void Initialize(TypeScroll type=TypeScroll.None)
        {
            if (this.type == TypeScroll.None)
            {
                this.type = type;
            }
            List<int> splitData_Free;
            List<int> splitData_Ad;
            switch (this.type)
            {
                case TypeScroll.DolPass:
                splitData_Free = GetDolPassSplitData(SeolPassServerTable.MonthlypassFreeReward_dol);
                splitData_Ad = GetDolPassSplitData(SeolPassServerTable.MonthlypassAdReward_dol);
                viewCount = 2.2f;
                    break;
                case TypeScroll.MonthPass:
                    splitData_Free = GetMonthPassSplitData(MonthlyPassServerTable.MonthlypassFreeReward);
                    splitData_Ad = GetMonthPassSplitData(MonthlyPassServerTable.MonthlypassAdReward);
                    viewCount = 6f;
                    break;
                case TypeScroll.MonthPass2:
                    splitData_Free = GetMonthPass2SplitData(MonthlyPassServerTable2.MonthlypassFreeReward);
                    splitData_Ad = GetMonthPass2SplitData(MonthlyPassServerTable2.MonthlypassAdReward);
                    viewCount = 6f;
                    break;
                case TypeScroll.SnowManPass:
                    splitData_Free = GetSnowAttenPassSplitData(OneYearPassServerTable.childFree);
                    splitData_Ad = GetSnowAttenPassSplitData(OneYearPassServerTable.childAd);
                    viewCount = 6f;
                    break;
                case TypeScroll.SuhoPass:
                    splitData_Free = GetSuhoPassSplitData(ColdSeasonPassServerTable.suhoFree);
                    splitData_Ad = GetSuhoPassSplitData(ColdSeasonPassServerTable.suhoAd);
                    viewCount = 6f;
                    break;
                case TypeScroll.FoxFirePass:
                    splitData_Free = GetSuhoPassSplitData(ColdSeasonPassServerTable.foxfireFree);
                    splitData_Ad = GetSuhoPassSplitData(ColdSeasonPassServerTable.foxfireAd);
                    viewCount = 6f;
                    break;
                case TypeScroll.SealSwordPass:
                    splitData_Free = GetSuhoPassSplitData(ColdSeasonPassServerTable.sealSwordFree);
                    splitData_Ad = GetSuhoPassSplitData(ColdSeasonPassServerTable.sealSwordAd);
                    viewCount = 6f;
                    //0.86f 50개
                    //0.906f 70
                    //0.933f
                    break;
                case TypeScroll.InventoryView:
                    splitData_Free = null;
                    splitData_Ad = null;
                    viewCount = 7;
                    break;
                case TypeScroll.InventoryOnlyView:
                    splitData_Free = null;
                    splitData_Ad = null;
                    viewCount = 4f;
                    break;
                case TypeScroll.OnlyEffectView:
                    splitData_Free = null;
                    splitData_Ad = null;
                    viewCount = 4f;
                    break;
                default:
                    splitData_Free = null;
                    splitData_Ad = null;
                    viewCount = 2;
                    break;
            }

            if (totalCount != 0)
            {
                maxX = (float)(totalCount - viewCount) / (float)totalCount;
            }
            int freeMax = 0;
            int AdMax = 0;
            if (splitData_Free != null && splitData_Free.Count != 0)
            {
                freeMax =  splitData_Free.Max();
            }
            if (splitData_Ad != null && splitData_Ad.Count != 0)
            {
                AdMax = splitData_Ad.Max();
            }
            //int totalCount = TableManager.Instance.dolPass.dataArray.Length;
            if (scrollbar != null)
            {
                if (this.type == TypeScroll.MonthPass2)
                {
                    scrollbar.value = Mathf.Clamp01(Mathf.Max(freeMax,0.7f) / Mathf.Max(totalCount - 1f, 1e-4f));
                }
                else
                {
                    scrollbar.value = Mathf.Clamp01(Mathf.Max(freeMax,3) / Mathf.Max(totalCount - 1f, 1e-4f));
                }
            }
        }
        public void OnValueChanged(Action<float> callback) => onValueChanged = callback;

        public void OnSelectionChanged(Action<int> callback) => onSelectionChanged = callback;

        public void SetTotalCount(int totalCount) => this.totalCount = totalCount;

        public void ScrollTo(float position, float duration, Action onComplete = null) => ScrollTo(position, duration, Ease.OutCubic, onComplete);

        public void ScrollTo(float position, float duration, Ease easing, Action onComplete = null) => ScrollTo(position, duration, Easing.Get(easing), onComplete);

        public void ScrollTo(float position, float duration, EasingFunction easingFunction, Action onComplete = null)
        {
            if (duration <= 0f)
            {
                Position = CircularPosition(position, totalCount);
                onComplete?.Invoke();
                return;
            }

            autoScrollState.Reset();
            autoScrollState.Enable = true;
            autoScrollState.Duration = duration;
            autoScrollState.EasingFunction = easingFunction ?? DefaultEasingFunction;
            autoScrollState.StartTime = Time.unscaledTime;
            autoScrollState.EndPosition = currentPosition + CalculateMovementAmount(currentPosition, position);
            autoScrollState.OnComplete = onComplete;

            velocity = 0f;
            scrollStartPosition = currentPosition;

            UpdateSelection(Mathf.RoundToInt(CircularPosition(autoScrollState.EndPosition, totalCount)));
        }

        public void JumpTo(int index)
        {
            if (index < 0 || index > totalCount - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            UpdateSelection(index);
            Position = index;
        }

        public MovementDirection GetMovementDirection(int sourceIndex, int destIndex)
        {
            var movementAmount = CalculateMovementAmount(sourceIndex, destIndex);
            return scrollDirection == ScrollDirection.Horizontal
                ? movementAmount > 0
                    ? MovementDirection.Left
                    : MovementDirection.Right
                : movementAmount > 0
                    ? MovementDirection.Up
                    : MovementDirection.Down;
        }

        /// <inheritdoc/>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            hold = true;
            velocity = 0f;
            autoScrollState.Reset();
        }

        /// <inheritdoc/>
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (hold && snap.Enable)
            {
                UpdateSelection(Mathf.Clamp(Mathf.RoundToInt(currentPosition), 0, totalCount - 1));
                ScrollTo(Mathf.RoundToInt(currentPosition), snap.Duration, snap.Easing);
            }

            hold = false;
        }

        /// <inheritdoc/>
        void IScrollHandler.OnScroll(PointerEventData eventData)
        {
            if (!draggable)
            {
                return;
            }

            var delta = eventData.scrollDelta;

            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            var scrollDelta = scrollDirection == ScrollDirection.Horizontal
                ? Mathf.Abs(delta.y) > Mathf.Abs(delta.x)
                        ? delta.y
                        : delta.x
                : Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
                        ? delta.x
                        : delta.y;

            if (eventData.IsScrolling())
            {
                scrolling = true;
            }

            var position = currentPosition + scrollDelta / ViewportSize * scrollSensitivity;
            if (movementType == MovementType.Clamped)
            {
                position += CalculateOffset(position);
            }

            if (autoScrollState.Enable)
            {
                autoScrollState.Reset();
            }

            UpdatePosition(position);
        }

        /// <inheritdoc/>
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            hold = false;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewport,
                eventData.position,
                eventData.pressEventCamera,
                out beginDragPointerPosition);

            scrollStartPosition = currentPosition;
            dragging = true;
            autoScrollState.Reset();
        }

        /// <inheritdoc/>
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left || !dragging)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewport,
                eventData.position,
                eventData.pressEventCamera,
                out var dragPointerPosition))
            {
                return;
            }

            var pointerDelta = dragPointerPosition - beginDragPointerPosition;
            var position = (scrollDirection == ScrollDirection.Horizontal ? -pointerDelta.x : pointerDelta.y)
                           / ViewportSize
                           * scrollSensitivity
                           + scrollStartPosition;

            var offset = CalculateOffset(position);
            position += offset;

            if (movementType == MovementType.Elastic)
            {
                if (offset != 0f)
                {
                    position -= RubberDelta(offset, scrollSensitivity);
                }
            }

            UpdatePosition(Mathf.Min(position, totalCount-viewCount));
            //UpdatePosition(position);
        }

        /// <inheritdoc/>
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            dragging = false;
        }

        float CalculateOffset(float position)
        {
            if (movementType == MovementType.Unrestricted)
            {
                return 0f;
            }

            if (position < 0f)
            {
                return -position;
            }

            if (position > totalCount - 1)
            {
                return totalCount - 1 - position;
            }

            return 0f;
        }

        void UpdatePosition(float position, bool updateScrollbar = true)
        {
            onValueChanged?.Invoke(currentPosition = position);

            if (scrollbar && updateScrollbar)
            {
                scrollbar.value = Mathf.Clamp01(position / Mathf.Max(totalCount - 1f, 1e-4f));
            }
        }

        void UpdateSelection(int index) => onSelectionChanged?.Invoke(index);

        float RubberDelta(float overStretching, float viewSize) =>
            (1 - 1 / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1)) * viewSize * Mathf.Sign(overStretching);

        void Update()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var offset = CalculateOffset(currentPosition);

            if (autoScrollState.Enable)
            {
                var position = 0f;

                if (autoScrollState.Elastic)
                {
                    position = Mathf.SmoothDamp(currentPosition, currentPosition + offset, ref velocity,
                        elasticity, Mathf.Infinity, deltaTime);

                    if (Mathf.Abs(velocity) < 0.01f)
                    {
                        position = Mathf.Clamp(Mathf.RoundToInt(position), 0, totalCount - 1);
                        velocity = 0f;
                        autoScrollState.Complete();
                    }
                }
                else
                {
                    var alpha = Mathf.Clamp01((Time.unscaledTime - autoScrollState.StartTime) /
                                               Mathf.Max(autoScrollState.Duration, float.Epsilon));
                    position = Mathf.LerpUnclamped(scrollStartPosition, autoScrollState.EndPosition,
                        autoScrollState.EasingFunction(alpha));

                    if (Mathf.Approximately(alpha, 1f))
                    {
                        autoScrollState.Complete();
                    }
                }

                UpdatePosition(position);
            }
            else if (!(dragging || scrolling) && (!Mathf.Approximately(offset, 0f) || !Mathf.Approximately(velocity, 0f)))
            {
                var position = currentPosition;

                if (movementType == MovementType.Elastic && !Mathf.Approximately(offset, 0f))
                {
                    autoScrollState.Reset();
                    autoScrollState.Enable = true;
                    autoScrollState.Elastic = true;

                    UpdateSelection(Mathf.Clamp(Mathf.RoundToInt(position), 0, totalCount - 1));
                }
                else if (inertia)
                {
                    velocity *= Mathf.Pow(decelerationRate, deltaTime);

                    if (Mathf.Abs(velocity) < 0.001f)
                    {
                        velocity = 0f;
                    }

                    if (position > totalCount - viewCount)
                    {
                        velocity = 0f;
                    }
                    position += velocity * deltaTime;

                    if (snap.Enable && Mathf.Abs(velocity) < snap.VelocityThreshold)
                    {
                        ScrollTo(Mathf.RoundToInt(currentPosition), snap.Duration, snap.Easing);
                    }
                }
                else
                {
                    velocity = 0f;
                }

                if (!Mathf.Approximately(velocity, 0f))
                {
                    if (movementType == MovementType.Clamped)
                    {
                        offset = CalculateOffset(position);
                        position += offset;

                        if (Mathf.Approximately(position, 0f) || Mathf.Approximately(position, totalCount - 1f))
                        {
                            velocity = 0f;
                            UpdateSelection(Mathf.RoundToInt(position));
                        }
                    }

                    UpdatePosition(position);
                }
            }

            if (!autoScrollState.Enable && (dragging || scrolling) && inertia)
            {
                var newVelocity = (currentPosition - prevPosition) / deltaTime;
                velocity = Mathf.Lerp(velocity, newVelocity, deltaTime * 10f);
            }

            prevPosition = currentPosition;
            scrolling = false;
        }

        float CalculateMovementAmount(float sourcePosition, float destPosition)
        {
            if (movementType != MovementType.Unrestricted)
            {
                return Mathf.Clamp(destPosition, 0, totalCount - 1) - sourcePosition;
            }

            var amount = CircularPosition(destPosition, totalCount) - CircularPosition(sourcePosition, totalCount);

            if (Mathf.Abs(amount) > totalCount * 0.5f)
            {
                amount = Mathf.Sign(-amount) * (totalCount - Mathf.Abs(amount));
            }

            return amount;
        }

        float CircularPosition(float p, int size) => size < 1 ? 0 : p < 0 ? size - 1 + (p + 1) % size : p % size;
    }
}