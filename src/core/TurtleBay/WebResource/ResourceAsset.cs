﻿using WebExpress.Attribute;

namespace TurtleBay.WebResource
{
    /// <summary>
    /// Lieferung einer im Assamby eingebetteten Ressource
    /// </summary>
    [ID("Asset")]
    [Title("Assets")]
    [Segment("assets", "")]
    [Path("/")]
    [IncludeSubPaths(true)]
    [Module("TurtleBay")]
    public sealed class ResourceAsset : WebExpress.WebResource.ResourceAsset
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ResourceAsset()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
        }
    }
}
