namespace H3Lib
{
    /// <summary>
    /// Digit representing overage type
    /// </summary>
    public enum Overage
    {
        /// <summary>
        /// No overage
        /// </summary>
        NoOverage = 0,
        /// <summary>
        /// Overage at face edge
        /// </summary>
        FaceEdge = 1,
        /// <summary>
        /// Overage goes on next face
        /// </summary>
        NewFace = 2
    }
}
