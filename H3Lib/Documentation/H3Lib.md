<a name='assembly'></a>
# H3Lib

## Contents

- [Algos](#T-H3Lib-Constants-Algos 'H3Lib.Constants.Algos')
  - [Directions](#F-H3Lib-Constants-Algos-Directions 'H3Lib.Constants.Algos.Directions')
  - [NewAdjustmentIi](#F-H3Lib-Constants-Algos-NewAdjustmentIi 'H3Lib.Constants.Algos.NewAdjustmentIi')
  - [NewAdjustmentIii](#F-H3Lib-Constants-Algos-NewAdjustmentIii 'H3Lib.Constants.Algos.NewAdjustmentIii')
  - [NewDigitIi](#F-H3Lib-Constants-Algos-NewDigitIi 'H3Lib.Constants.Algos.NewDigitIi')
  - [NewDigitIii](#F-H3Lib-Constants-Algos-NewDigitIii 'H3Lib.Constants.Algos.NewDigitIii')
  - [NextRingDirection](#F-H3Lib-Constants-Algos-NextRingDirection 'H3Lib.Constants.Algos.NextRingDirection')
- [Api](#T-H3Lib-Api 'H3Lib.Api')
  - [CellAreaKm2()](#M-H3Lib-Api-CellAreaKm2-H3Lib-H3Index- 'H3Lib.Api.CellAreaKm2(H3Lib.H3Index)')
  - [CellAreaM2()](#M-H3Lib-Api-CellAreaM2-H3Lib-H3Index- 'H3Lib.Api.CellAreaM2(H3Lib.H3Index)')
  - [CellAreaRads2()](#M-H3Lib-Api-CellAreaRads2-H3Lib-H3Index- 'H3Lib.Api.CellAreaRads2(H3Lib.H3Index)')
  - [Compact()](#M-H3Lib-Api-Compact-System-Collections-Generic-List{H3Lib-H3Index},System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.Compact(System.Collections.Generic.List{H3Lib.H3Index},System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [DegreesToRadians(degrees)](#M-H3Lib-Api-DegreesToRadians-System-Double- 'H3Lib.Api.DegreesToRadians(System.Double)')
  - [EdgeLengthKm()](#M-H3Lib-Api-EdgeLengthKm-System-Int32- 'H3Lib.Api.EdgeLengthKm(System.Int32)')
  - [EdgeLengthM()](#M-H3Lib-Api-EdgeLengthM-System-Int32- 'H3Lib.Api.EdgeLengthM(System.Int32)')
  - [ExactEdgeLengthKm()](#M-H3Lib-Api-ExactEdgeLengthKm-H3Lib-H3Index- 'H3Lib.Api.ExactEdgeLengthKm(H3Lib.H3Index)')
  - [ExactEdgeLengthM()](#M-H3Lib-Api-ExactEdgeLengthM-H3Lib-H3Index- 'H3Lib.Api.ExactEdgeLengthM(H3Lib.H3Index)')
  - [ExactEdgeLengthRads()](#M-H3Lib-Api-ExactEdgeLengthRads-H3Lib-H3Index- 'H3Lib.Api.ExactEdgeLengthRads(H3Lib.H3Index)')
  - [ExperimentalH3ToLocalIj()](#M-H3Lib-Api-ExperimentalH3ToLocalIj-H3Lib-H3Index,H3Lib-H3Index,H3Lib-CoordIj@- 'H3Lib.Api.ExperimentalH3ToLocalIj(H3Lib.H3Index,H3Lib.H3Index,H3Lib.CoordIj@)')
  - [ExperimentalLocalIjToH3()](#M-H3Lib-Api-ExperimentalLocalIjToH3-H3Lib-H3Index,H3Lib-CoordIj,H3Lib-H3Index@- 'H3Lib.Api.ExperimentalLocalIjToH3(H3Lib.H3Index,H3Lib.CoordIj,H3Lib.H3Index@)')
  - [GeoToH3()](#M-H3Lib-Api-GeoToH3-H3Lib-GeoCoord,System-Int32- 'H3Lib.Api.GeoToH3(H3Lib.GeoCoord,System.Int32)')
  - [GetDestinationH3IndexFromUnidirectionalEdge()](#M-H3Lib-Api-GetDestinationH3IndexFromUnidirectionalEdge-H3Lib-H3Index- 'H3Lib.Api.GetDestinationH3IndexFromUnidirectionalEdge(H3Lib.H3Index)')
  - [GetH3IndexesFromUnidirectionalEdge()](#M-H3Lib-Api-GetH3IndexesFromUnidirectionalEdge-H3Lib-H3Index,System-ValueTuple{H3Lib-H3Index,H3Lib-H3Index}@- 'H3Lib.Api.GetH3IndexesFromUnidirectionalEdge(H3Lib.H3Index,System.ValueTuple{H3Lib.H3Index,H3Lib.H3Index}@)')
  - [GetH3UnidirectionalEdge()](#M-H3Lib-Api-GetH3UnidirectionalEdge-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Api.GetH3UnidirectionalEdge(H3Lib.H3Index,H3Lib.H3Index)')
  - [GetH3UnidirectionalEdgeBoundary()](#M-H3Lib-Api-GetH3UnidirectionalEdgeBoundary-H3Lib-H3Index,H3Lib-GeoBoundary@- 'H3Lib.Api.GetH3UnidirectionalEdgeBoundary(H3Lib.H3Index,H3Lib.GeoBoundary@)')
  - [GetH3UnidirectionalEdgesFromHexagon()](#M-H3Lib-Api-GetH3UnidirectionalEdgesFromHexagon-H3Lib-H3Index,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.GetH3UnidirectionalEdgesFromHexagon(H3Lib.H3Index,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [GetOriginH3IndexFromUnidirectionalEdge()](#M-H3Lib-Api-GetOriginH3IndexFromUnidirectionalEdge-H3Lib-H3Index- 'H3Lib.Api.GetOriginH3IndexFromUnidirectionalEdge(H3Lib.H3Index)')
  - [GetPentagonIndexes()](#M-H3Lib-Api-GetPentagonIndexes-System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.GetPentagonIndexes(System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [GetRes0Indexes()](#M-H3Lib-Api-GetRes0Indexes-System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.GetRes0Indexes(System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [H3Distance()](#M-H3Lib-Api-H3Distance-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Api.H3Distance(H3Lib.H3Index,H3Lib.H3Index)')
  - [H3GetBaseCell()](#M-H3Lib-Api-H3GetBaseCell-H3Lib-H3Index- 'H3Lib.Api.H3GetBaseCell(H3Lib.H3Index)')
  - [H3GetFaces()](#M-H3Lib-Api-H3GetFaces-H3Lib-H3Index,System-Collections-Generic-List{System-Int32}@- 'H3Lib.Api.H3GetFaces(H3Lib.H3Index,System.Collections.Generic.List{System.Int32}@)')
  - [H3GetResolution()](#M-H3Lib-Api-H3GetResolution-H3Lib-H3Index- 'H3Lib.Api.H3GetResolution(H3Lib.H3Index)')
  - [H3IndexesAreNeighbors()](#M-H3Lib-Api-H3IndexesAreNeighbors-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Api.H3IndexesAreNeighbors(H3Lib.H3Index,H3Lib.H3Index)')
  - [H3IsPentagon()](#M-H3Lib-Api-H3IsPentagon-H3Lib-H3Index- 'H3Lib.Api.H3IsPentagon(H3Lib.H3Index)')
  - [H3IsResClassIii()](#M-H3Lib-Api-H3IsResClassIii-H3Lib-H3Index- 'H3Lib.Api.H3IsResClassIii(H3Lib.H3Index)')
  - [H3IsValid()](#M-H3Lib-Api-H3IsValid-H3Lib-H3Index- 'H3Lib.Api.H3IsValid(H3Lib.H3Index)')
  - [H3Line()](#M-H3Lib-Api-H3Line-H3Lib-H3Index,H3Lib-H3Index,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.H3Line(H3Lib.H3Index,H3Lib.H3Index,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [H3LineSize()](#M-H3Lib-Api-H3LineSize-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Api.H3LineSize(H3Lib.H3Index,H3Lib.H3Index)')
  - [H3SetToLinkedGeo(h3Set,outPolygon)](#M-H3Lib-Api-H3SetToLinkedGeo-System-Collections-Generic-List{H3Lib-H3Index},H3Lib-LinkedGeoPolygon@- 'H3Lib.Api.H3SetToLinkedGeo(System.Collections.Generic.List{H3Lib.H3Index},H3Lib.LinkedGeoPolygon@)')
  - [H3ToCenterChild()](#M-H3Lib-Api-H3ToCenterChild-H3Lib-H3Index,System-Int32- 'H3Lib.Api.H3ToCenterChild(H3Lib.H3Index,System.Int32)')
  - [H3ToChildren()](#M-H3Lib-Api-H3ToChildren-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.H3ToChildren(H3Lib.H3Index,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [H3ToGeo()](#M-H3Lib-Api-H3ToGeo-H3Lib-H3Index,H3Lib-GeoCoord@- 'H3Lib.Api.H3ToGeo(H3Lib.H3Index,H3Lib.GeoCoord@)')
  - [H3ToGeoBoundary()](#M-H3Lib-Api-H3ToGeoBoundary-H3Lib-H3Index,H3Lib-GeoBoundary@- 'H3Lib.Api.H3ToGeoBoundary(H3Lib.H3Index,H3Lib.GeoBoundary@)')
  - [H3ToParent()](#M-H3Lib-Api-H3ToParent-H3Lib-H3Index,System-Int32- 'H3Lib.Api.H3ToParent(H3Lib.H3Index,System.Int32)')
  - [H3ToString()](#M-H3Lib-Api-H3ToString-H3Lib-H3Index,System-String@- 'H3Lib.Api.H3ToString(H3Lib.H3Index,System.String@)')
  - [H3UnidirectionalEdgeIsValid()](#M-H3Lib-Api-H3UnidirectionalEdgeIsValid-H3Lib-H3Index- 'H3Lib.Api.H3UnidirectionalEdgeIsValid(H3Lib.H3Index)')
  - [HexAreaKm2()](#M-H3Lib-Api-HexAreaKm2-System-Int32- 'H3Lib.Api.HexAreaKm2(System.Int32)')
  - [HexAreaM2()](#M-H3Lib-Api-HexAreaM2-System-Int32- 'H3Lib.Api.HexAreaM2(System.Int32)')
  - [HexRange()](#M-H3Lib-Api-HexRange-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.HexRange(H3Lib.H3Index,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [HexRangeDistances()](#M-H3Lib-Api-HexRangeDistances-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@,System-Collections-Generic-List{System-Int32}@- 'H3Lib.Api.HexRangeDistances(H3Lib.H3Index,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@,System.Collections.Generic.List{System.Int32}@)')
  - [HexRanges()](#M-H3Lib-Api-HexRanges-System-Collections-Generic-List{H3Lib-H3Index},System-Int32,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.HexRanges(System.Collections.Generic.List{H3Lib.H3Index},System.Int32,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [HexRing(origin,k,outCells)](#M-H3Lib-Api-HexRing-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.HexRing(H3Lib.H3Index,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [KRing()](#M-H3Lib-Api-KRing-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.KRing(H3Lib.H3Index,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [KRingDistances()](#M-H3Lib-Api-KRingDistances-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@,System-Collections-Generic-List{System-Int32}@- 'H3Lib.Api.KRingDistances(H3Lib.H3Index,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@,System.Collections.Generic.List{System.Int32}@)')
  - [MaxFaceCount()](#M-H3Lib-Api-MaxFaceCount-H3Lib-H3Index- 'H3Lib.Api.MaxFaceCount(H3Lib.H3Index)')
  - [MaxH3ToChildrenSize()](#M-H3Lib-Api-MaxH3ToChildrenSize-H3Lib-H3Index,System-Int32- 'H3Lib.Api.MaxH3ToChildrenSize(H3Lib.H3Index,System.Int32)')
  - [MaxKringSize()](#M-H3Lib-Api-MaxKringSize-System-Int32- 'H3Lib.Api.MaxKringSize(System.Int32)')
  - [MaxPolyFillSize(polygon,r)](#M-H3Lib-Api-MaxPolyFillSize-H3Lib-GeoPolygon,System-Int32- 'H3Lib.Api.MaxPolyFillSize(H3Lib.GeoPolygon,System.Int32)')
  - [MaxUncompactSize()](#M-H3Lib-Api-MaxUncompactSize-H3Lib-H3Index,System-Int32- 'H3Lib.Api.MaxUncompactSize(H3Lib.H3Index,System.Int32)')
  - [NumHexagons()](#M-H3Lib-Api-NumHexagons-System-Int32- 'H3Lib.Api.NumHexagons(System.Int32)')
  - [PentagonIndexCount()](#M-H3Lib-Api-PentagonIndexCount 'H3Lib.Api.PentagonIndexCount')
  - [PointDistKm(a,b)](#M-H3Lib-Api-PointDistKm-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Api.PointDistKm(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [PointDistM()](#M-H3Lib-Api-PointDistM-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Api.PointDistM(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [PointDistRads(a,b)](#M-H3Lib-Api-PointDistRads-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Api.PointDistRads(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [PolyFill(polygon,r,outCells)](#M-H3Lib-Api-PolyFill-H3Lib-GeoPolygon,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Api.PolyFill(H3Lib.GeoPolygon,System.Int32,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [RadiansToDegrees()](#M-H3Lib-Api-RadiansToDegrees-System-Double- 'H3Lib.Api.RadiansToDegrees(System.Double)')
  - [Res0IndexCount()](#M-H3Lib-Api-Res0IndexCount 'H3Lib.Api.Res0IndexCount')
  - [SetGeoDegs()](#M-H3Lib-Api-SetGeoDegs-System-Double,System-Double- 'H3Lib.Api.SetGeoDegs(System.Double,System.Double)')
  - [StringToH3()](#M-H3Lib-Api-StringToH3-System-String- 'H3Lib.Api.StringToH3(System.String)')
  - [Uncompact()](#M-H3Lib-Api-Uncompact-System-Collections-Generic-List{H3Lib-H3Index},System-Collections-Generic-List{H3Lib-H3Index}@,System-Int32- 'H3Lib.Api.Uncompact(System.Collections.Generic.List{H3Lib.H3Index},System.Collections.Generic.List{H3Lib.H3Index}@,System.Int32)')
- [BBox](#T-H3Lib-BBox 'H3Lib.BBox')
  - [#ctor()](#M-H3Lib-BBox-#ctor-System-Double,System-Double,System-Double,System-Double- 'H3Lib.BBox.#ctor(System.Double,System.Double,System.Double,System.Double)')
  - [East](#F-H3Lib-BBox-East 'H3Lib.BBox.East')
  - [North](#F-H3Lib-BBox-North 'H3Lib.BBox.North')
  - [South](#F-H3Lib-BBox-South 'H3Lib.BBox.South')
  - [West](#F-H3Lib-BBox-West 'H3Lib.BBox.West')
  - [IsTransmeridian](#P-H3Lib-BBox-IsTransmeridian 'H3Lib.BBox.IsTransmeridian')
  - [Equals()](#M-H3Lib-BBox-Equals-H3Lib-BBox- 'H3Lib.BBox.Equals(H3Lib.BBox)')
  - [Equals()](#M-H3Lib-BBox-Equals-System-Object- 'H3Lib.BBox.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-BBox-GetHashCode 'H3Lib.BBox.GetHashCode')
  - [op_Equality()](#M-H3Lib-BBox-op_Equality-H3Lib-BBox,H3Lib-BBox- 'H3Lib.BBox.op_Equality(H3Lib.BBox,H3Lib.BBox)')
  - [op_Inequality()](#M-H3Lib-BBox-op_Inequality-H3Lib-BBox,H3Lib-BBox- 'H3Lib.BBox.op_Inequality(H3Lib.BBox,H3Lib.BBox)')
- [BBoxExtensions](#T-H3Lib-Extensions-BBoxExtensions 'H3Lib.Extensions.BBoxExtensions')
  - [Center(box)](#M-H3Lib-Extensions-BBoxExtensions-Center-H3Lib-BBox- 'H3Lib.Extensions.BBoxExtensions.Center(H3Lib.BBox)')
  - [Contains(box,point)](#M-H3Lib-Extensions-BBoxExtensions-Contains-H3Lib-BBox,H3Lib-GeoCoord- 'H3Lib.Extensions.BBoxExtensions.Contains(H3Lib.BBox,H3Lib.GeoCoord)')
  - [HexEstimate(box,res)](#M-H3Lib-Extensions-BBoxExtensions-HexEstimate-H3Lib-BBox,System-Int32- 'H3Lib.Extensions.BBoxExtensions.HexEstimate(H3Lib.BBox,System.Int32)')
  - [ReplaceEW(box,e,w)](#M-H3Lib-Extensions-BBoxExtensions-ReplaceEW-H3Lib-BBox,System-Double,System-Double- 'H3Lib.Extensions.BBoxExtensions.ReplaceEW(H3Lib.BBox,System.Double,System.Double)')
  - [ReplaceEast(box,e)](#M-H3Lib-Extensions-BBoxExtensions-ReplaceEast-H3Lib-BBox,System-Double- 'H3Lib.Extensions.BBoxExtensions.ReplaceEast(H3Lib.BBox,System.Double)')
  - [ReplaceNorth(box,n)](#M-H3Lib-Extensions-BBoxExtensions-ReplaceNorth-H3Lib-BBox,System-Double- 'H3Lib.Extensions.BBoxExtensions.ReplaceNorth(H3Lib.BBox,System.Double)')
  - [ReplaceSouth(box,s)](#M-H3Lib-Extensions-BBoxExtensions-ReplaceSouth-H3Lib-BBox,System-Double- 'H3Lib.Extensions.BBoxExtensions.ReplaceSouth(H3Lib.BBox,System.Double)')
  - [ReplaceWest(box,w)](#M-H3Lib-Extensions-BBoxExtensions-ReplaceWest-H3Lib-BBox,System-Double- 'H3Lib.Extensions.BBoxExtensions.ReplaceWest(H3Lib.BBox,System.Double)')
- [BaseCellData](#T-H3Lib-BaseCellData 'H3Lib.BaseCellData')
  - [#ctor(face,faceI,faceJ,faceK,isPentagon,offset1,offset2)](#M-H3Lib-BaseCellData-#ctor-System-Int32,System-Int32,System-Int32,System-Int32,System-Int32,System-Int32,System-Int32- 'H3Lib.BaseCellData.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)')
  - [ClockwiseOffsetPentagon](#F-H3Lib-BaseCellData-ClockwiseOffsetPentagon 'H3Lib.BaseCellData.ClockwiseOffsetPentagon')
  - [HomeFijk](#F-H3Lib-BaseCellData-HomeFijk 'H3Lib.BaseCellData.HomeFijk')
  - [IsPentagon](#F-H3Lib-BaseCellData-IsPentagon 'H3Lib.BaseCellData.IsPentagon')
  - [Equals(other)](#M-H3Lib-BaseCellData-Equals-H3Lib-BaseCellData- 'H3Lib.BaseCellData.Equals(H3Lib.BaseCellData)')
  - [Equals(obj)](#M-H3Lib-BaseCellData-Equals-System-Object- 'H3Lib.BaseCellData.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-BaseCellData-GetHashCode 'H3Lib.BaseCellData.GetHashCode')
  - [op_Equality(left,right)](#M-H3Lib-BaseCellData-op_Equality-H3Lib-BaseCellData,H3Lib-BaseCellData- 'H3Lib.BaseCellData.op_Equality(H3Lib.BaseCellData,H3Lib.BaseCellData)')
  - [op_Inequality(left,right)](#M-H3Lib-BaseCellData-op_Inequality-H3Lib-BaseCellData,H3Lib-BaseCellData- 'H3Lib.BaseCellData.op_Inequality(H3Lib.BaseCellData,H3Lib.BaseCellData)')
- [BaseCellRotation](#T-H3Lib-BaseCellRotation 'H3Lib.BaseCellRotation')
  - [#ctor()](#M-H3Lib-BaseCellRotation-#ctor-System-Int32,System-Int32- 'H3Lib.BaseCellRotation.#ctor(System.Int32,System.Int32)')
  - [BaseCell](#F-H3Lib-BaseCellRotation-BaseCell 'H3Lib.BaseCellRotation.BaseCell')
  - [CounterClockwiseRotate60](#F-H3Lib-BaseCellRotation-CounterClockwiseRotate60 'H3Lib.BaseCellRotation.CounterClockwiseRotate60')
  - [Equals()](#M-H3Lib-BaseCellRotation-Equals-H3Lib-BaseCellRotation- 'H3Lib.BaseCellRotation.Equals(H3Lib.BaseCellRotation)')
  - [Equals()](#M-H3Lib-BaseCellRotation-Equals-System-Object- 'H3Lib.BaseCellRotation.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-BaseCellRotation-GetHashCode 'H3Lib.BaseCellRotation.GetHashCode')
  - [op_Equality()](#M-H3Lib-BaseCellRotation-op_Equality-H3Lib-BaseCellRotation,H3Lib-BaseCellRotation- 'H3Lib.BaseCellRotation.op_Equality(H3Lib.BaseCellRotation,H3Lib.BaseCellRotation)')
  - [op_Inequality()](#M-H3Lib-BaseCellRotation-op_Inequality-H3Lib-BaseCellRotation,H3Lib-BaseCellRotation- 'H3Lib.BaseCellRotation.op_Inequality(H3Lib.BaseCellRotation,H3Lib.BaseCellRotation)')
- [BaseCells](#T-H3Lib-Constants-BaseCells 'H3Lib.Constants.BaseCells')
  - [BaseCellData](#F-H3Lib-Constants-BaseCells-BaseCellData 'H3Lib.Constants.BaseCells.BaseCellData')
  - [BaseCellNeighbor60CounterClockwiseRotation](#F-H3Lib-Constants-BaseCells-BaseCellNeighbor60CounterClockwiseRotation 'H3Lib.Constants.BaseCells.BaseCellNeighbor60CounterClockwiseRotation')
  - [BaseCellNeighbors](#F-H3Lib-Constants-BaseCells-BaseCellNeighbors 'H3Lib.Constants.BaseCells.BaseCellNeighbors')
  - [FaceIjkBaseCells](#F-H3Lib-Constants-BaseCells-FaceIjkBaseCells 'H3Lib.Constants.BaseCells.FaceIjkBaseCells')
  - [InvalidRotations](#F-H3Lib-Constants-BaseCells-InvalidRotations 'H3Lib.Constants.BaseCells.InvalidRotations')
  - [MaxFaceCoord](#F-H3Lib-Constants-BaseCells-MaxFaceCoord 'H3Lib.Constants.BaseCells.MaxFaceCoord')
- [BaseCellsExtensions](#T-H3Lib-Extensions-BaseCellsExtensions 'H3Lib.Extensions.BaseCellsExtensions')
  - [Res0IndexCount](#P-H3Lib-Extensions-BaseCellsExtensions-Res0IndexCount 'H3Lib.Extensions.BaseCellsExtensions.Res0IndexCount')
  - [GetBaseCellDirection()](#M-H3Lib-Extensions-BaseCellsExtensions-GetBaseCellDirection-System-Int32,System-Int32- 'H3Lib.Extensions.BaseCellsExtensions.GetBaseCellDirection(System.Int32,System.Int32)')
  - [GetNeighbor()](#M-H3Lib-Extensions-BaseCellsExtensions-GetNeighbor-System-Int32,H3Lib-Direction- 'H3Lib.Extensions.BaseCellsExtensions.GetNeighbor(System.Int32,H3Lib.Direction)')
  - [GetRes0Indexes()](#M-H3Lib-Extensions-BaseCellsExtensions-GetRes0Indexes 'H3Lib.Extensions.BaseCellsExtensions.GetRes0Indexes')
  - [IsBaseCellPentagon()](#M-H3Lib-Extensions-BaseCellsExtensions-IsBaseCellPentagon-System-Int32- 'H3Lib.Extensions.BaseCellsExtensions.IsBaseCellPentagon(System.Int32)')
  - [IsBaseCellPolarPentagon()](#M-H3Lib-Extensions-BaseCellsExtensions-IsBaseCellPolarPentagon-System-Int32- 'H3Lib.Extensions.BaseCellsExtensions.IsBaseCellPolarPentagon(System.Int32)')
  - [IsClockwiseOffset()](#M-H3Lib-Extensions-BaseCellsExtensions-IsClockwiseOffset-System-Int32,System-Int32- 'H3Lib.Extensions.BaseCellsExtensions.IsClockwiseOffset(System.Int32,System.Int32)')
  - [ToCounterClockwiseRotate60()](#M-H3Lib-Extensions-BaseCellsExtensions-ToCounterClockwiseRotate60-System-Int32,System-Int32- 'H3Lib.Extensions.BaseCellsExtensions.ToCounterClockwiseRotate60(System.Int32,System.Int32)')
  - [ToFaceIjk()](#M-H3Lib-Extensions-BaseCellsExtensions-ToFaceIjk-System-Int32- 'H3Lib.Extensions.BaseCellsExtensions.ToFaceIjk(System.Int32)')
- [CollectionExtensions](#T-H3Lib-Extensions-CollectionExtensions 'H3Lib.Extensions.CollectionExtensions')
  - [FindDeepestContainer(polygons,boxes)](#M-H3Lib-Extensions-CollectionExtensions-FindDeepestContainer-System-Collections-Generic-List{H3Lib-LinkedGeoPolygon},System-Collections-Generic-List{H3Lib-BBox}- 'H3Lib.Extensions.CollectionExtensions.FindDeepestContainer(System.Collections.Generic.List{H3Lib.LinkedGeoPolygon},System.Collections.Generic.List{H3Lib.BBox})')
  - [HexRanges(h3Set,k)](#M-H3Lib-Extensions-CollectionExtensions-HexRanges-System-Collections-Generic-List{H3Lib-H3Index},System-Int32- 'H3Lib.Extensions.CollectionExtensions.HexRanges(System.Collections.Generic.List{H3Lib.H3Index},System.Int32)')
  - [MaxUncompactSize(compactedSet,res)](#M-H3Lib-Extensions-CollectionExtensions-MaxUncompactSize-System-Collections-Generic-List{H3Lib-H3Index},System-Int32- 'H3Lib.Extensions.CollectionExtensions.MaxUncompactSize(System.Collections.Generic.List{H3Lib.H3Index},System.Int32)')
  - [ToLinkedGeoPolygon(h3Set)](#M-H3Lib-Extensions-CollectionExtensions-ToLinkedGeoPolygon-System-Collections-Generic-List{H3Lib-H3Index}- 'H3Lib.Extensions.CollectionExtensions.ToLinkedGeoPolygon(System.Collections.Generic.List{H3Lib.H3Index})')
  - [ToVertexGraph(h3Set)](#M-H3Lib-Extensions-CollectionExtensions-ToVertexGraph-System-Collections-Generic-List{H3Lib-H3Index}- 'H3Lib.Extensions.CollectionExtensions.ToVertexGraph(System.Collections.Generic.List{H3Lib.H3Index})')
  - [Uncompact(compactedSet,res)](#M-H3Lib-Extensions-CollectionExtensions-Uncompact-System-Collections-Generic-List{H3Lib-H3Index},System-Int32- 'H3Lib.Extensions.CollectionExtensions.Uncompact(System.Collections.Generic.List{H3Lib.H3Index},System.Int32)')
- [Constants](#T-H3Lib-Constants 'H3Lib.Constants')
  - [H3_VERSION_MAJOR](#F-H3Lib-Constants-H3_VERSION_MAJOR 'H3Lib.Constants.H3_VERSION_MAJOR')
  - [H3_VERSION_MINOR](#F-H3Lib-Constants-H3_VERSION_MINOR 'H3Lib.Constants.H3_VERSION_MINOR')
  - [H3_VERSION_PATCH](#F-H3Lib-Constants-H3_VERSION_PATCH 'H3Lib.Constants.H3_VERSION_PATCH')
- [CoordIj](#T-H3Lib-CoordIj 'H3Lib.CoordIj')
  - [#ctor()](#M-H3Lib-CoordIj-#ctor-System-Int32,System-Int32- 'H3Lib.CoordIj.#ctor(System.Int32,System.Int32)')
  - [#ctor()](#M-H3Lib-CoordIj-#ctor-H3Lib-CoordIj- 'H3Lib.CoordIj.#ctor(H3Lib.CoordIj)')
  - [I](#F-H3Lib-CoordIj-I 'H3Lib.CoordIj.I')
  - [J](#F-H3Lib-CoordIj-J 'H3Lib.CoordIj.J')
  - [Equals()](#M-H3Lib-CoordIj-Equals-H3Lib-CoordIj- 'H3Lib.CoordIj.Equals(H3Lib.CoordIj)')
  - [Equals()](#M-H3Lib-CoordIj-Equals-System-Object- 'H3Lib.CoordIj.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-CoordIj-GetHashCode 'H3Lib.CoordIj.GetHashCode')
  - [op_Addition()](#M-H3Lib-CoordIj-op_Addition-H3Lib-CoordIj,H3Lib-CoordIj- 'H3Lib.CoordIj.op_Addition(H3Lib.CoordIj,H3Lib.CoordIj)')
  - [op_Equality()](#M-H3Lib-CoordIj-op_Equality-H3Lib-CoordIj,H3Lib-CoordIj- 'H3Lib.CoordIj.op_Equality(H3Lib.CoordIj,H3Lib.CoordIj)')
  - [op_Inequality()](#M-H3Lib-CoordIj-op_Inequality-H3Lib-CoordIj,H3Lib-CoordIj- 'H3Lib.CoordIj.op_Inequality(H3Lib.CoordIj,H3Lib.CoordIj)')
  - [op_Multiply()](#M-H3Lib-CoordIj-op_Multiply-H3Lib-CoordIj,System-Int32- 'H3Lib.CoordIj.op_Multiply(H3Lib.CoordIj,System.Int32)')
  - [op_Subtraction()](#M-H3Lib-CoordIj-op_Subtraction-H3Lib-CoordIj,H3Lib-CoordIj- 'H3Lib.CoordIj.op_Subtraction(H3Lib.CoordIj,H3Lib.CoordIj)')
- [CoordIjExtensions](#T-H3Lib-Extensions-CoordIjExtensions 'H3Lib.Extensions.CoordIjExtensions')
  - [ReplaceI()](#M-H3Lib-Extensions-CoordIjExtensions-ReplaceI-H3Lib-CoordIj,System-Int32- 'H3Lib.Extensions.CoordIjExtensions.ReplaceI(H3Lib.CoordIj,System.Int32)')
  - [ReplaceJ()](#M-H3Lib-Extensions-CoordIjExtensions-ReplaceJ-H3Lib-CoordIj,System-Int32- 'H3Lib.Extensions.CoordIjExtensions.ReplaceJ(H3Lib.CoordIj,System.Int32)')
  - [ToH3Experimental(ij,origin)](#M-H3Lib-Extensions-CoordIjExtensions-ToH3Experimental-H3Lib-CoordIj,H3Lib-H3Index- 'H3Lib.Extensions.CoordIjExtensions.ToH3Experimental(H3Lib.CoordIj,H3Lib.H3Index)')
  - [ToIjk(ij)](#M-H3Lib-Extensions-CoordIjExtensions-ToIjk-H3Lib-CoordIj- 'H3Lib.Extensions.CoordIjExtensions.ToIjk(H3Lib.CoordIj)')
- [CoordIjk](#T-H3Lib-Constants-CoordIjk 'H3Lib.Constants.CoordIjk')
- [CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk')
  - [#ctor()](#M-H3Lib-CoordIjk-#ctor-System-Int32,System-Int32,System-Int32- 'H3Lib.CoordIjk.#ctor(System.Int32,System.Int32,System.Int32)')
  - [#ctor()](#M-H3Lib-CoordIjk-#ctor-H3Lib-CoordIjk- 'H3Lib.CoordIjk.#ctor(H3Lib.CoordIjk)')
  - [UnitVecs](#F-H3Lib-Constants-CoordIjk-UnitVecs 'H3Lib.Constants.CoordIjk.UnitVecs')
  - [I](#F-H3Lib-CoordIjk-I 'H3Lib.CoordIjk.I')
  - [J](#F-H3Lib-CoordIjk-J 'H3Lib.CoordIjk.J')
  - [K](#F-H3Lib-CoordIjk-K 'H3Lib.CoordIjk.K')
  - [CubeRound(i,j,k)](#M-H3Lib-CoordIjk-CubeRound-System-Double,System-Double,System-Double- 'H3Lib.CoordIjk.CubeRound(System.Double,System.Double,System.Double)')
  - [Equals()](#M-H3Lib-CoordIjk-Equals-H3Lib-CoordIjk- 'H3Lib.CoordIjk.Equals(H3Lib.CoordIjk)')
  - [Equals()](#M-H3Lib-CoordIjk-Equals-System-Object- 'H3Lib.CoordIjk.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-CoordIjk-GetHashCode 'H3Lib.CoordIjk.GetHashCode')
  - [ToString()](#M-H3Lib-CoordIjk-ToString 'H3Lib.CoordIjk.ToString')
  - [op_Addition()](#M-H3Lib-CoordIjk-op_Addition-H3Lib-CoordIjk,H3Lib-CoordIjk- 'H3Lib.CoordIjk.op_Addition(H3Lib.CoordIjk,H3Lib.CoordIjk)')
  - [op_Equality()](#M-H3Lib-CoordIjk-op_Equality-H3Lib-CoordIjk,H3Lib-CoordIjk- 'H3Lib.CoordIjk.op_Equality(H3Lib.CoordIjk,H3Lib.CoordIjk)')
  - [op_Inequality()](#M-H3Lib-CoordIjk-op_Inequality-H3Lib-CoordIjk,H3Lib-CoordIjk- 'H3Lib.CoordIjk.op_Inequality(H3Lib.CoordIjk,H3Lib.CoordIjk)')
  - [op_Multiply()](#M-H3Lib-CoordIjk-op_Multiply-H3Lib-CoordIjk,System-Int32- 'H3Lib.CoordIjk.op_Multiply(H3Lib.CoordIjk,System.Int32)')
  - [op_Subtraction()](#M-H3Lib-CoordIjk-op_Subtraction-H3Lib-CoordIjk,H3Lib-CoordIjk- 'H3Lib.CoordIjk.op_Subtraction(H3Lib.CoordIjk,H3Lib.CoordIjk)')
- [CoordIjkExtensions](#T-H3Lib-Extensions-CoordIjkExtensions 'H3Lib.Extensions.CoordIjkExtensions')
  - [DistanceTo(start,end)](#M-H3Lib-Extensions-CoordIjkExtensions-DistanceTo-H3Lib-CoordIjk,H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.DistanceTo(H3Lib.CoordIjk,H3Lib.CoordIjk)')
  - [DownAp3(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-DownAp3-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.DownAp3(H3Lib.CoordIjk)')
  - [DownAp3R(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-DownAp3R-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.DownAp3R(H3Lib.CoordIjk)')
  - [DownAp7(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-DownAp7-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.DownAp7(H3Lib.CoordIjk)')
  - [DownAp7R(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-DownAp7R-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.DownAp7R(H3Lib.CoordIjk)')
  - [FromCube(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-FromCube-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.FromCube(H3Lib.CoordIjk)')
  - [IsZero()](#M-H3Lib-Extensions-CoordIjkExtensions-IsZero-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.IsZero(H3Lib.CoordIjk)')
  - [LocalIjkToH3(origin,ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-LocalIjkToH3-H3Lib-CoordIjk,H3Lib-H3Index- 'H3Lib.Extensions.CoordIjkExtensions.LocalIjkToH3(H3Lib.CoordIjk,H3Lib.H3Index)')
  - [Neighbor(ijk,digit)](#M-H3Lib-Extensions-CoordIjkExtensions-Neighbor-H3Lib-CoordIjk,H3Lib-Direction- 'H3Lib.Extensions.CoordIjkExtensions.Neighbor(H3Lib.CoordIjk,H3Lib.Direction)')
  - [Normalized(coord)](#M-H3Lib-Extensions-CoordIjkExtensions-Normalized-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.Normalized(H3Lib.CoordIjk)')
  - [Rotate60Clockwise(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-Rotate60Clockwise-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.Rotate60Clockwise(H3Lib.CoordIjk)')
  - [Rotate60CounterClockwise(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-Rotate60CounterClockwise-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.Rotate60CounterClockwise(H3Lib.CoordIjk)')
  - [SetI()](#M-H3Lib-Extensions-CoordIjkExtensions-SetI-H3Lib-CoordIjk,System-Int32- 'H3Lib.Extensions.CoordIjkExtensions.SetI(H3Lib.CoordIjk,System.Int32)')
  - [SetIJ()](#M-H3Lib-Extensions-CoordIjkExtensions-SetIJ-H3Lib-CoordIjk,System-Int32,System-Int32- 'H3Lib.Extensions.CoordIjkExtensions.SetIJ(H3Lib.CoordIjk,System.Int32,System.Int32)')
  - [SetIK()](#M-H3Lib-Extensions-CoordIjkExtensions-SetIK-H3Lib-CoordIjk,System-Int32,System-Int32- 'H3Lib.Extensions.CoordIjkExtensions.SetIK(H3Lib.CoordIjk,System.Int32,System.Int32)')
  - [SetJ()](#M-H3Lib-Extensions-CoordIjkExtensions-SetJ-H3Lib-CoordIjk,System-Int32- 'H3Lib.Extensions.CoordIjkExtensions.SetJ(H3Lib.CoordIjk,System.Int32)')
  - [SetJK()](#M-H3Lib-Extensions-CoordIjkExtensions-SetJK-H3Lib-CoordIjk,System-Int32,System-Int32- 'H3Lib.Extensions.CoordIjkExtensions.SetJK(H3Lib.CoordIjk,System.Int32,System.Int32)')
  - [SetK()](#M-H3Lib-Extensions-CoordIjkExtensions-SetK-H3Lib-CoordIjk,System-Int32- 'H3Lib.Extensions.CoordIjkExtensions.SetK(H3Lib.CoordIjk,System.Int32)')
  - [Sum()](#M-H3Lib-Extensions-CoordIjkExtensions-Sum-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.Sum(H3Lib.CoordIjk)')
  - [ToCube(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-ToCube-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.ToCube(H3Lib.CoordIjk)')
  - [ToDirection(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-ToDirection-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.ToDirection(H3Lib.CoordIjk)')
  - [ToHex2d(h)](#M-H3Lib-Extensions-CoordIjkExtensions-ToHex2d-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.ToHex2d(H3Lib.CoordIjk)')
  - [ToIj(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-ToIj-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.ToIj(H3Lib.CoordIjk)')
  - [UpAp7(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-UpAp7-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.UpAp7(H3Lib.CoordIjk)')
  - [UpAp7R(ijk)](#M-H3Lib-Extensions-CoordIjkExtensions-UpAp7R-H3Lib-CoordIjk- 'H3Lib.Extensions.CoordIjkExtensions.UpAp7R(H3Lib.CoordIjk)')
- [Direction](#T-H3Lib-Direction 'H3Lib.Direction')
  - [CENTER_DIGIT](#F-H3Lib-Direction-CENTER_DIGIT 'H3Lib.Direction.CENTER_DIGIT')
  - [IJ_AXES_DIGIT](#F-H3Lib-Direction-IJ_AXES_DIGIT 'H3Lib.Direction.IJ_AXES_DIGIT')
  - [IK_AXES_DIGIT](#F-H3Lib-Direction-IK_AXES_DIGIT 'H3Lib.Direction.IK_AXES_DIGIT')
  - [INVALID_DIGIT](#F-H3Lib-Direction-INVALID_DIGIT 'H3Lib.Direction.INVALID_DIGIT')
  - [I_AXES_DIGIT](#F-H3Lib-Direction-I_AXES_DIGIT 'H3Lib.Direction.I_AXES_DIGIT')
  - [JK_AXES_DIGIT](#F-H3Lib-Direction-JK_AXES_DIGIT 'H3Lib.Direction.JK_AXES_DIGIT')
  - [J_AXES_DIGIT](#F-H3Lib-Direction-J_AXES_DIGIT 'H3Lib.Direction.J_AXES_DIGIT')
  - [K_AXES_DIGIT](#F-H3Lib-Direction-K_AXES_DIGIT 'H3Lib.Direction.K_AXES_DIGIT')
  - [NUM_DIGITS](#F-H3Lib-Direction-NUM_DIGITS 'H3Lib.Direction.NUM_DIGITS')
- [DirectionExtensions](#T-H3Lib-Extensions-DirectionExtensions 'H3Lib.Extensions.DirectionExtensions')
  - [Rotate60Clockwise(digit)](#M-H3Lib-Extensions-DirectionExtensions-Rotate60Clockwise-H3Lib-Direction- 'H3Lib.Extensions.DirectionExtensions.Rotate60Clockwise(H3Lib.Direction)')
  - [Rotate60CounterClockwise(digit)](#M-H3Lib-Extensions-DirectionExtensions-Rotate60CounterClockwise-H3Lib-Direction- 'H3Lib.Extensions.DirectionExtensions.Rotate60CounterClockwise(H3Lib.Direction)')
- [FaceIjk](#T-H3Lib-Constants-FaceIjk 'H3Lib.Constants.FaceIjk')
- [FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk')
  - [#ctor()](#M-H3Lib-FaceIjk-#ctor-System-Int32,H3Lib-CoordIjk- 'H3Lib.FaceIjk.#ctor(System.Int32,H3Lib.CoordIjk)')
  - [#ctor()](#M-H3Lib-FaceIjk-#ctor-H3Lib-FaceIjk- 'H3Lib.FaceIjk.#ctor(H3Lib.FaceIjk)')
  - [AdjacentFaceDir](#F-H3Lib-Constants-FaceIjk-AdjacentFaceDir 'H3Lib.Constants.FaceIjk.AdjacentFaceDir')
  - [FaceAxesAzRadsCii](#F-H3Lib-Constants-FaceIjk-FaceAxesAzRadsCii 'H3Lib.Constants.FaceIjk.FaceAxesAzRadsCii')
  - [FaceCenterGeo](#F-H3Lib-Constants-FaceIjk-FaceCenterGeo 'H3Lib.Constants.FaceIjk.FaceCenterGeo')
  - [FaceCenterPoint](#F-H3Lib-Constants-FaceIjk-FaceCenterPoint 'H3Lib.Constants.FaceIjk.FaceCenterPoint')
  - [FaceNeighbors](#F-H3Lib-Constants-FaceIjk-FaceNeighbors 'H3Lib.Constants.FaceIjk.FaceNeighbors')
  - [IJ](#F-H3Lib-Constants-FaceIjk-IJ 'H3Lib.Constants.FaceIjk.IJ')
  - [InvalidFace](#F-H3Lib-Constants-FaceIjk-InvalidFace 'H3Lib.Constants.FaceIjk.InvalidFace')
  - [JK](#F-H3Lib-Constants-FaceIjk-JK 'H3Lib.Constants.FaceIjk.JK')
  - [KI](#F-H3Lib-Constants-FaceIjk-KI 'H3Lib.Constants.FaceIjk.KI')
  - [MSqrt7](#F-H3Lib-Constants-FaceIjk-MSqrt7 'H3Lib.Constants.FaceIjk.MSqrt7')
  - [MaxDimByCiiRes](#F-H3Lib-Constants-FaceIjk-MaxDimByCiiRes 'H3Lib.Constants.FaceIjk.MaxDimByCiiRes')
  - [UnitScaleByCiiRes](#F-H3Lib-Constants-FaceIjk-UnitScaleByCiiRes 'H3Lib.Constants.FaceIjk.UnitScaleByCiiRes')
  - [Coord](#F-H3Lib-FaceIjk-Coord 'H3Lib.FaceIjk.Coord')
  - [Face](#F-H3Lib-FaceIjk-Face 'H3Lib.FaceIjk.Face')
  - [Equals()](#M-H3Lib-FaceIjk-Equals-H3Lib-FaceIjk- 'H3Lib.FaceIjk.Equals(H3Lib.FaceIjk)')
  - [Equals()](#M-H3Lib-FaceIjk-Equals-System-Object- 'H3Lib.FaceIjk.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-FaceIjk-GetHashCode 'H3Lib.FaceIjk.GetHashCode')
  - [ToString()](#M-H3Lib-FaceIjk-ToString 'H3Lib.FaceIjk.ToString')
  - [op_Equality()](#M-H3Lib-FaceIjk-op_Equality-H3Lib-FaceIjk,H3Lib-FaceIjk- 'H3Lib.FaceIjk.op_Equality(H3Lib.FaceIjk,H3Lib.FaceIjk)')
  - [op_Inequality()](#M-H3Lib-FaceIjk-op_Inequality-H3Lib-FaceIjk,H3Lib-FaceIjk- 'H3Lib.FaceIjk.op_Inequality(H3Lib.FaceIjk,H3Lib.FaceIjk)')
- [FaceIjkExtensions](#T-H3Lib-Extensions-FaceIjkExtensions 'H3Lib.Extensions.FaceIjkExtensions')
  - [AdjustOverageClassIi(fijk,res,pentLeading4,substrate)](#M-H3Lib-Extensions-FaceIjkExtensions-AdjustOverageClassIi-H3Lib-FaceIjk,System-Int32,System-Int32,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.AdjustOverageClassIi(H3Lib.FaceIjk,System.Int32,System.Int32,System.Int32)')
  - [AdjustPentOverage(fijk,res)](#M-H3Lib-Extensions-FaceIjkExtensions-AdjustPentOverage-H3Lib-FaceIjk,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.AdjustPentOverage(H3Lib.FaceIjk,System.Int32)')
  - [PentToGeoBoundary(h,res,start,length)](#M-H3Lib-Extensions-FaceIjkExtensions-PentToGeoBoundary-H3Lib-FaceIjk,System-Int32,System-Int32,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.PentToGeoBoundary(H3Lib.FaceIjk,System.Int32,System.Int32,System.Int32)')
  - [PentToVerts(fijk,res,fijkVerts)](#M-H3Lib-Extensions-FaceIjkExtensions-PentToVerts-H3Lib-FaceIjk,System-Int32,System-Collections-Generic-IList{H3Lib-FaceIjk}- 'H3Lib.Extensions.FaceIjkExtensions.PentToVerts(H3Lib.FaceIjk,System.Int32,System.Collections.Generic.IList{H3Lib.FaceIjk})')
  - [ReplaceCoord(fijk,coord)](#M-H3Lib-Extensions-FaceIjkExtensions-ReplaceCoord-H3Lib-FaceIjk,H3Lib-CoordIjk- 'H3Lib.Extensions.FaceIjkExtensions.ReplaceCoord(H3Lib.FaceIjk,H3Lib.CoordIjk)')
  - [ReplaceFace(fijk,face)](#M-H3Lib-Extensions-FaceIjkExtensions-ReplaceFace-H3Lib-FaceIjk,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.ReplaceFace(H3Lib.FaceIjk,System.Int32)')
  - [ToBaseCell()](#M-H3Lib-Extensions-FaceIjkExtensions-ToBaseCell-H3Lib-FaceIjk- 'H3Lib.Extensions.FaceIjkExtensions.ToBaseCell(H3Lib.FaceIjk)')
  - [ToBaseCellCounterClockwiseRotate60()](#M-H3Lib-Extensions-FaceIjkExtensions-ToBaseCellCounterClockwiseRotate60-H3Lib-FaceIjk- 'H3Lib.Extensions.FaceIjkExtensions.ToBaseCellCounterClockwiseRotate60(H3Lib.FaceIjk)')
  - [ToGeoBoundary(h,res,start,length)](#M-H3Lib-Extensions-FaceIjkExtensions-ToGeoBoundary-H3Lib-FaceIjk,System-Int32,System-Int32,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.ToGeoBoundary(H3Lib.FaceIjk,System.Int32,System.Int32,System.Int32)')
  - [ToGeoCoord(h,res)](#M-H3Lib-Extensions-FaceIjkExtensions-ToGeoCoord-H3Lib-FaceIjk,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.ToGeoCoord(H3Lib.FaceIjk,System.Int32)')
  - [ToH3(fijk,res)](#M-H3Lib-Extensions-FaceIjkExtensions-ToH3-H3Lib-FaceIjk,System-Int32- 'H3Lib.Extensions.FaceIjkExtensions.ToH3(H3Lib.FaceIjk,System.Int32)')
  - [ToVerts(fijk,res,fijkVerts)](#M-H3Lib-Extensions-FaceIjkExtensions-ToVerts-H3Lib-FaceIjk,System-Int32,System-Collections-Generic-IList{H3Lib-FaceIjk}- 'H3Lib.Extensions.FaceIjkExtensions.ToVerts(H3Lib.FaceIjk,System.Int32,System.Collections.Generic.IList{H3Lib.FaceIjk})')
- [FaceOrientIjk](#T-H3Lib-FaceOrientIjk 'H3Lib.FaceOrientIjk')
  - [#ctor()](#M-H3Lib-FaceOrientIjk-#ctor-System-Int32,System-Int32,System-Int32,System-Int32,System-Int32- 'H3Lib.FaceOrientIjk.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)')
  - [#ctor()](#M-H3Lib-FaceOrientIjk-#ctor-System-Int32,H3Lib-CoordIjk,System-Int32- 'H3Lib.FaceOrientIjk.#ctor(System.Int32,H3Lib.CoordIjk,System.Int32)')
  - [Ccw60Rotations](#F-H3Lib-FaceOrientIjk-Ccw60Rotations 'H3Lib.FaceOrientIjk.Ccw60Rotations')
  - [Face](#F-H3Lib-FaceOrientIjk-Face 'H3Lib.FaceOrientIjk.Face')
  - [Translate](#F-H3Lib-FaceOrientIjk-Translate 'H3Lib.FaceOrientIjk.Translate')
  - [Equals(other)](#M-H3Lib-FaceOrientIjk-Equals-H3Lib-FaceOrientIjk- 'H3Lib.FaceOrientIjk.Equals(H3Lib.FaceOrientIjk)')
  - [Equals()](#M-H3Lib-FaceOrientIjk-Equals-System-Object- 'H3Lib.FaceOrientIjk.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-FaceOrientIjk-GetHashCode 'H3Lib.FaceOrientIjk.GetHashCode')
  - [op_Equality()](#M-H3Lib-FaceOrientIjk-op_Equality-H3Lib-FaceOrientIjk,H3Lib-FaceOrientIjk- 'H3Lib.FaceOrientIjk.op_Equality(H3Lib.FaceOrientIjk,H3Lib.FaceOrientIjk)')
  - [op_Inequality()](#M-H3Lib-FaceOrientIjk-op_Inequality-H3Lib-FaceOrientIjk,H3Lib-FaceOrientIjk- 'H3Lib.FaceOrientIjk.op_Inequality(H3Lib.FaceOrientIjk,H3Lib.FaceOrientIjk)')
- [GeoBoundary](#T-H3Lib-GeoBoundary 'H3Lib.GeoBoundary')
  - [#ctor()](#M-H3Lib-GeoBoundary-#ctor 'H3Lib.GeoBoundary.#ctor')
  - [NumVerts](#F-H3Lib-GeoBoundary-NumVerts 'H3Lib.GeoBoundary.NumVerts')
  - [Verts](#F-H3Lib-GeoBoundary-Verts 'H3Lib.GeoBoundary.Verts')
  - [ToString()](#M-H3Lib-GeoBoundary-ToString 'H3Lib.GeoBoundary.ToString')
- [GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord')
  - [#ctor()](#M-H3Lib-GeoCoord-#ctor-System-Double,System-Double- 'H3Lib.GeoCoord.#ctor(System.Double,System.Double)')
  - [#ctor()](#M-H3Lib-GeoCoord-#ctor-H3Lib-GeoCoord- 'H3Lib.GeoCoord.#ctor(H3Lib.GeoCoord)')
  - [Latitude](#F-H3Lib-GeoCoord-Latitude 'H3Lib.GeoCoord.Latitude')
  - [Longitude](#F-H3Lib-GeoCoord-Longitude 'H3Lib.GeoCoord.Longitude')
  - [EdgeLengthKm()](#M-H3Lib-GeoCoord-EdgeLengthKm-System-Int32- 'H3Lib.GeoCoord.EdgeLengthKm(System.Int32)')
  - [EdgeLengthM()](#M-H3Lib-GeoCoord-EdgeLengthM-System-Int32- 'H3Lib.GeoCoord.EdgeLengthM(System.Int32)')
  - [Equals()](#M-H3Lib-GeoCoord-Equals-H3Lib-GeoCoord- 'H3Lib.GeoCoord.Equals(H3Lib.GeoCoord)')
  - [Equals()](#M-H3Lib-GeoCoord-Equals-System-Object- 'H3Lib.GeoCoord.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-GeoCoord-GetHashCode 'H3Lib.GeoCoord.GetHashCode')
  - [HexAreaKm2()](#M-H3Lib-GeoCoord-HexAreaKm2-System-Int32- 'H3Lib.GeoCoord.HexAreaKm2(System.Int32)')
  - [HexAreaM2()](#M-H3Lib-GeoCoord-HexAreaM2-System-Int32- 'H3Lib.GeoCoord.HexAreaM2(System.Int32)')
  - [ToString()](#M-H3Lib-GeoCoord-ToString 'H3Lib.GeoCoord.ToString')
  - [TriangleArea(a,b,c)](#M-H3Lib-GeoCoord-TriangleArea-H3Lib-GeoCoord,H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.GeoCoord.TriangleArea(H3Lib.GeoCoord,H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [TriangleEdgeLengthToArea(a,b,c)](#M-H3Lib-GeoCoord-TriangleEdgeLengthToArea-System-Double,System-Double,System-Double- 'H3Lib.GeoCoord.TriangleEdgeLengthToArea(System.Double,System.Double,System.Double)')
  - [op_Equality()](#M-H3Lib-GeoCoord-op_Equality-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.GeoCoord.op_Equality(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [op_Inequality()](#M-H3Lib-GeoCoord-op_Inequality-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.GeoCoord.op_Inequality(H3Lib.GeoCoord,H3Lib.GeoCoord)')
- [GeoCoordExtensions](#T-H3Lib-Extensions-GeoCoordExtensions 'H3Lib.Extensions.GeoCoordExtensions')
  - [AzimuthRadiansTo(p1,p2)](#M-H3Lib-Extensions-GeoCoordExtensions-AzimuthRadiansTo-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Extensions.GeoCoordExtensions.AzimuthRadiansTo(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [DistanceToKm(a,b)](#M-H3Lib-Extensions-GeoCoordExtensions-DistanceToKm-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Extensions.GeoCoordExtensions.DistanceToKm(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [DistanceToM(a,b)](#M-H3Lib-Extensions-GeoCoordExtensions-DistanceToM-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Extensions.GeoCoordExtensions.DistanceToM(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [DistanceToRadians(a,b)](#M-H3Lib-Extensions-GeoCoordExtensions-DistanceToRadians-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.Extensions.GeoCoordExtensions.DistanceToRadians(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [GetAzimuthDistancePoint(p1,azimuth,distance)](#M-H3Lib-Extensions-GeoCoordExtensions-GetAzimuthDistancePoint-H3Lib-GeoCoord,System-Double,System-Double- 'H3Lib.Extensions.GeoCoordExtensions.GetAzimuthDistancePoint(H3Lib.GeoCoord,System.Double,System.Double)')
  - [LineHexEstimate(origin,destination,res)](#M-H3Lib-Extensions-GeoCoordExtensions-LineHexEstimate-H3Lib-GeoCoord,H3Lib-GeoCoord,System-Int32- 'H3Lib.Extensions.GeoCoordExtensions.LineHexEstimate(H3Lib.GeoCoord,H3Lib.GeoCoord,System.Int32)')
  - [SetDegrees(gc,latitude,longitude)](#M-H3Lib-Extensions-GeoCoordExtensions-SetDegrees-H3Lib-GeoCoord,System-Double,System-Double- 'H3Lib.Extensions.GeoCoordExtensions.SetDegrees(H3Lib.GeoCoord,System.Double,System.Double)')
  - [SetGeoRads(gc,latitudeRadians,longitudeRadians)](#M-H3Lib-Extensions-GeoCoordExtensions-SetGeoRads-H3Lib-GeoCoord,System-Double,System-Double- 'H3Lib.Extensions.GeoCoordExtensions.SetGeoRads(H3Lib.GeoCoord,System.Double,System.Double)')
  - [SetLatitude()](#M-H3Lib-Extensions-GeoCoordExtensions-SetLatitude-H3Lib-GeoCoord,System-Double- 'H3Lib.Extensions.GeoCoordExtensions.SetLatitude(H3Lib.GeoCoord,System.Double)')
  - [SetLongitude()](#M-H3Lib-Extensions-GeoCoordExtensions-SetLongitude-H3Lib-GeoCoord,System-Double- 'H3Lib.Extensions.GeoCoordExtensions.SetLongitude(H3Lib.GeoCoord,System.Double)')
  - [SetRadians(gc,latitude,longitude)](#M-H3Lib-Extensions-GeoCoordExtensions-SetRadians-H3Lib-GeoCoord,System-Double,System-Double- 'H3Lib.Extensions.GeoCoordExtensions.SetRadians(H3Lib.GeoCoord,System.Double,System.Double)')
  - [ToFaceIjk(g,res)](#M-H3Lib-Extensions-GeoCoordExtensions-ToFaceIjk-H3Lib-GeoCoord,System-Int32- 'H3Lib.Extensions.GeoCoordExtensions.ToFaceIjk(H3Lib.GeoCoord,System.Int32)')
  - [ToH3Index(g,res)](#M-H3Lib-Extensions-GeoCoordExtensions-ToH3Index-H3Lib-GeoCoord,System-Int32- 'H3Lib.Extensions.GeoCoordExtensions.ToH3Index(H3Lib.GeoCoord,System.Int32)')
  - [ToHex2d(g,res)](#M-H3Lib-Extensions-GeoCoordExtensions-ToHex2d-H3Lib-GeoCoord,System-Int32- 'H3Lib.Extensions.GeoCoordExtensions.ToHex2d(H3Lib.GeoCoord,System.Int32)')
  - [ToVec3d(geo)](#M-H3Lib-Extensions-GeoCoordExtensions-ToVec3d-H3Lib-GeoCoord- 'H3Lib.Extensions.GeoCoordExtensions.ToVec3d(H3Lib.GeoCoord)')
- [GeoFence](#T-H3Lib-GeoFence 'H3Lib.GeoFence')
  - [#ctor()](#M-H3Lib-GeoFence-#ctor 'H3Lib.GeoFence.#ctor')
  - [NumVerts](#F-H3Lib-GeoFence-NumVerts 'H3Lib.GeoFence.NumVerts')
  - [Verts](#F-H3Lib-GeoFence-Verts 'H3Lib.GeoFence.Verts')
  - [IsEmpty](#P-H3Lib-GeoFence-IsEmpty 'H3Lib.GeoFence.IsEmpty')
- [GeoFenceExtensions](#T-H3Lib-Extensions-GeoFenceExtensions 'H3Lib.Extensions.GeoFenceExtensions')
  - [GetEdgeHexagons(geofence,numHexagons,res,numSearchHexagons,search,found)](#M-H3Lib-Extensions-GeoFenceExtensions-GetEdgeHexagons-H3Lib-GeoFence,System-Int32,System-Int32,System-Int32@,System-Collections-Generic-List{H3Lib-H3Index}@,System-Collections-Generic-List{H3Lib-H3Index}@- 'H3Lib.Extensions.GeoFenceExtensions.GetEdgeHexagons(H3Lib.GeoFence,System.Int32,System.Int32,System.Int32@,System.Collections.Generic.List{H3Lib.H3Index}@,System.Collections.Generic.List{H3Lib.H3Index}@)')
  - [IsClockwise(loop)](#M-H3Lib-Extensions-GeoFenceExtensions-IsClockwise-H3Lib-GeoFence- 'H3Lib.Extensions.GeoFenceExtensions.IsClockwise(H3Lib.GeoFence)')
  - [IsClockwiseNormalized(loop,isTransmeridian)](#M-H3Lib-Extensions-GeoFenceExtensions-IsClockwiseNormalized-H3Lib-GeoFence,System-Boolean- 'H3Lib.Extensions.GeoFenceExtensions.IsClockwiseNormalized(H3Lib.GeoFence,System.Boolean)')
  - [PointInside(loop,box,coord)](#M-H3Lib-Extensions-GeoFenceExtensions-PointInside-H3Lib-GeoFence,H3Lib-BBox,H3Lib-GeoCoord- 'H3Lib.Extensions.GeoFenceExtensions.PointInside(H3Lib.GeoFence,H3Lib.BBox,H3Lib.GeoCoord)')
  - [ToBBox(loop)](#M-H3Lib-Extensions-GeoFenceExtensions-ToBBox-H3Lib-GeoFence- 'H3Lib.Extensions.GeoFenceExtensions.ToBBox(H3Lib.GeoFence)')
- [GeoMultiPolygon](#T-H3Lib-GeoMultiPolygon 'H3Lib.GeoMultiPolygon')
  - [NumPolygons](#F-H3Lib-GeoMultiPolygon-NumPolygons 'H3Lib.GeoMultiPolygon.NumPolygons')
  - [Polygons](#F-H3Lib-GeoMultiPolygon-Polygons 'H3Lib.GeoMultiPolygon.Polygons')
- [GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon')
  - [GeoFence](#F-H3Lib-GeoPolygon-GeoFence 'H3Lib.GeoPolygon.GeoFence')
  - [Holes](#F-H3Lib-GeoPolygon-Holes 'H3Lib.GeoPolygon.Holes')
  - [NumHoles](#F-H3Lib-GeoPolygon-NumHoles 'H3Lib.GeoPolygon.NumHoles')
- [GeoPolygonExtensions](#T-H3Lib-Extensions-GeoPolygonExtensions 'H3Lib.Extensions.GeoPolygonExtensions')
  - [MaxPolyFillSize(geoPolygon,res)](#M-H3Lib-Extensions-GeoPolygonExtensions-MaxPolyFillSize-H3Lib-GeoPolygon,System-Int32- 'H3Lib.Extensions.GeoPolygonExtensions.MaxPolyFillSize(H3Lib.GeoPolygon,System.Int32)')
  - [PointInside(polygon,boxes,coord)](#M-H3Lib-Extensions-GeoPolygonExtensions-PointInside-H3Lib-GeoPolygon,System-Collections-Generic-List{H3Lib-BBox},H3Lib-GeoCoord- 'H3Lib.Extensions.GeoPolygonExtensions.PointInside(H3Lib.GeoPolygon,System.Collections.Generic.List{H3Lib.BBox},H3Lib.GeoCoord)')
  - [PolyFillInternal(geoPolygon,res)](#M-H3Lib-Extensions-GeoPolygonExtensions-PolyFillInternal-H3Lib-GeoPolygon,System-Int32- 'H3Lib.Extensions.GeoPolygonExtensions.PolyFillInternal(H3Lib.GeoPolygon,System.Int32)')
  - [Polyfill(polygon,res)](#M-H3Lib-Extensions-GeoPolygonExtensions-Polyfill-H3Lib-GeoPolygon,System-Int32- 'H3Lib.Extensions.GeoPolygonExtensions.Polyfill(H3Lib.GeoPolygon,System.Int32)')
  - [ToBBoxes(polygon)](#M-H3Lib-Extensions-GeoPolygonExtensions-ToBBoxes-H3Lib-GeoPolygon- 'H3Lib.Extensions.GeoPolygonExtensions.ToBBoxes(H3Lib.GeoPolygon)')
- [H3](#T-H3Lib-Constants-H3 'H3Lib.Constants.H3')
  - [DBL_EPSILON](#F-H3Lib-Constants-H3-DBL_EPSILON 'H3Lib.Constants.H3.DBL_EPSILON')
  - [EARTH_RADIUS_KM](#F-H3Lib-Constants-H3-EARTH_RADIUS_KM 'H3Lib.Constants.H3.EARTH_RADIUS_KM')
  - [EPSILON](#F-H3Lib-Constants-H3-EPSILON 'H3Lib.Constants.H3.EPSILON')
  - [EPSILON_DEG](#F-H3Lib-Constants-H3-EPSILON_DEG 'H3Lib.Constants.H3.EPSILON_DEG')
  - [EPSILON_RAD](#F-H3Lib-Constants-H3-EPSILON_RAD 'H3Lib.Constants.H3.EPSILON_RAD')
  - [H3_HEXAGON_MODE](#F-H3Lib-Constants-H3-H3_HEXAGON_MODE 'H3Lib.Constants.H3.H3_HEXAGON_MODE')
  - [MAX_H3_RES](#F-H3Lib-Constants-H3-MAX_H3_RES 'H3Lib.Constants.H3.MAX_H3_RES')
  - [M_180_PI](#F-H3Lib-Constants-H3-M_180_PI 'H3Lib.Constants.H3.M_180_PI')
  - [M_2PI](#F-H3Lib-Constants-H3-M_2PI 'H3Lib.Constants.H3.M_2PI')
  - [M_AP7_ROT_RADS](#F-H3Lib-Constants-H3-M_AP7_ROT_RADS 'H3Lib.Constants.H3.M_AP7_ROT_RADS')
  - [M_COS_AP7_ROT](#F-H3Lib-Constants-H3-M_COS_AP7_ROT 'H3Lib.Constants.H3.M_COS_AP7_ROT')
  - [M_PI](#F-H3Lib-Constants-H3-M_PI 'H3Lib.Constants.H3.M_PI')
  - [M_PI_180](#F-H3Lib-Constants-H3-M_PI_180 'H3Lib.Constants.H3.M_PI_180')
  - [M_PI_2](#F-H3Lib-Constants-H3-M_PI_2 'H3Lib.Constants.H3.M_PI_2')
  - [M_SIN60](#F-H3Lib-Constants-H3-M_SIN60 'H3Lib.Constants.H3.M_SIN60')
  - [M_SIN_AP7_ROT](#F-H3Lib-Constants-H3-M_SIN_AP7_ROT 'H3Lib.Constants.H3.M_SIN_AP7_ROT')
  - [M_SQRT3_2](#F-H3Lib-Constants-H3-M_SQRT3_2 'H3Lib.Constants.H3.M_SQRT3_2')
  - [NEXT_RING_DIRECTION](#F-H3Lib-Constants-H3-NEXT_RING_DIRECTION 'H3Lib.Constants.H3.NEXT_RING_DIRECTION')
  - [NUM_BASE_CELLS](#F-H3Lib-Constants-H3-NUM_BASE_CELLS 'H3Lib.Constants.H3.NUM_BASE_CELLS')
  - [NUM_HEX_VERTS](#F-H3Lib-Constants-H3-NUM_HEX_VERTS 'H3Lib.Constants.H3.NUM_HEX_VERTS')
  - [NUM_ICOSA_FACES](#F-H3Lib-Constants-H3-NUM_ICOSA_FACES 'H3Lib.Constants.H3.NUM_ICOSA_FACES')
  - [NUM_PENT_VERTS](#F-H3Lib-Constants-H3-NUM_PENT_VERTS 'H3Lib.Constants.H3.NUM_PENT_VERTS')
  - [RES0_U_GNOMONIC](#F-H3Lib-Constants-H3-RES0_U_GNOMONIC 'H3Lib.Constants.H3.RES0_U_GNOMONIC')
- [H3Index](#T-H3Lib-Constants-H3Index 'H3Lib.Constants.H3Index')
- [H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index')
  - [#ctor(val)](#M-H3Lib-H3Index-#ctor-System-UInt64- 'H3Lib.H3Index.#ctor(System.UInt64)')
  - [#ctor()](#M-H3Lib-H3Index-#ctor-System-Int32,System-Int32,H3Lib-Direction- 'H3Lib.H3Index.#ctor(System.Int32,System.Int32,H3Lib.Direction)')
  - [#ctor(res,baseCell,initDigit)](#M-H3Lib-H3Index-#ctor-System-Int32,System-Int32,System-Int32- 'H3Lib.H3Index.#ctor(System.Int32,System.Int32,System.Int32)')
  - [H3_BC_MASK](#F-H3Lib-Constants-H3Index-H3_BC_MASK 'H3Lib.Constants.H3Index.H3_BC_MASK')
  - [H3_BC_MASK_NEGATIVE](#F-H3Lib-Constants-H3Index-H3_BC_MASK_NEGATIVE 'H3Lib.Constants.H3Index.H3_BC_MASK_NEGATIVE')
  - [H3_BC_OFFSET](#F-H3Lib-Constants-H3Index-H3_BC_OFFSET 'H3Lib.Constants.H3Index.H3_BC_OFFSET')
  - [H3_DIGIT_MASK](#F-H3Lib-Constants-H3Index-H3_DIGIT_MASK 'H3Lib.Constants.H3Index.H3_DIGIT_MASK')
  - [H3_DIGIT_MASK_NEGATIVE](#F-H3Lib-Constants-H3Index-H3_DIGIT_MASK_NEGATIVE 'H3Lib.Constants.H3Index.H3_DIGIT_MASK_NEGATIVE')
  - [H3_HIGH_BIT_MASK](#F-H3Lib-Constants-H3Index-H3_HIGH_BIT_MASK 'H3Lib.Constants.H3Index.H3_HIGH_BIT_MASK')
  - [H3_HIGH_BIT_MASK_NEGATIVE](#F-H3Lib-Constants-H3Index-H3_HIGH_BIT_MASK_NEGATIVE 'H3Lib.Constants.H3Index.H3_HIGH_BIT_MASK_NEGATIVE')
  - [H3_INIT](#F-H3Lib-Constants-H3Index-H3_INIT 'H3Lib.Constants.H3Index.H3_INIT')
  - [H3_INVALID_INDEX](#F-H3Lib-Constants-H3Index-H3_INVALID_INDEX 'H3Lib.Constants.H3Index.H3_INVALID_INDEX')
  - [H3_MAX_OFFSET](#F-H3Lib-Constants-H3Index-H3_MAX_OFFSET 'H3Lib.Constants.H3Index.H3_MAX_OFFSET')
  - [H3_MODE_MASK](#F-H3Lib-Constants-H3Index-H3_MODE_MASK 'H3Lib.Constants.H3Index.H3_MODE_MASK')
  - [H3_MODE_MASK_NEGATIVE](#F-H3Lib-Constants-H3Index-H3_MODE_MASK_NEGATIVE 'H3Lib.Constants.H3Index.H3_MODE_MASK_NEGATIVE')
  - [H3_MODE_OFFSET](#F-H3Lib-Constants-H3Index-H3_MODE_OFFSET 'H3Lib.Constants.H3Index.H3_MODE_OFFSET')
  - [H3_NULL](#F-H3Lib-Constants-H3Index-H3_NULL 'H3Lib.Constants.H3Index.H3_NULL')
  - [H3_NUM_BITS](#F-H3Lib-Constants-H3Index-H3_NUM_BITS 'H3Lib.Constants.H3Index.H3_NUM_BITS')
  - [H3_PER_DIGIT_OFFSET](#F-H3Lib-Constants-H3Index-H3_PER_DIGIT_OFFSET 'H3Lib.Constants.H3Index.H3_PER_DIGIT_OFFSET')
  - [H3_RESERVED_MASK](#F-H3Lib-Constants-H3Index-H3_RESERVED_MASK 'H3Lib.Constants.H3Index.H3_RESERVED_MASK')
  - [H3_RESERVED_MASK_NEGATIVE](#F-H3Lib-Constants-H3Index-H3_RESERVED_MASK_NEGATIVE 'H3Lib.Constants.H3Index.H3_RESERVED_MASK_NEGATIVE')
  - [H3_RESERVED_OFFSET](#F-H3Lib-Constants-H3Index-H3_RESERVED_OFFSET 'H3Lib.Constants.H3Index.H3_RESERVED_OFFSET')
  - [H3_RES_MASK](#F-H3Lib-Constants-H3Index-H3_RES_MASK 'H3Lib.Constants.H3Index.H3_RES_MASK')
  - [H3_RES_MASK_NEGATIVE](#F-H3Lib-Constants-H3Index-H3_RES_MASK_NEGATIVE 'H3Lib.Constants.H3Index.H3_RES_MASK_NEGATIVE')
  - [H3_RES_OFFSET](#F-H3Lib-Constants-H3Index-H3_RES_OFFSET 'H3Lib.Constants.H3Index.H3_RES_OFFSET')
  - [Value](#F-H3Lib-H3Index-Value 'H3Lib.H3Index.Value')
  - [BaseCell](#P-H3Lib-H3Index-BaseCell 'H3Lib.H3Index.BaseCell')
  - [HighBit](#P-H3Lib-H3Index-HighBit 'H3Lib.H3Index.HighBit')
  - [IsResClassIii](#P-H3Lib-H3Index-IsResClassIii 'H3Lib.H3Index.IsResClassIii')
  - [LeadingNonZeroDigit](#P-H3Lib-H3Index-LeadingNonZeroDigit 'H3Lib.H3Index.LeadingNonZeroDigit')
  - [Mode](#P-H3Lib-H3Index-Mode 'H3Lib.H3Index.Mode')
  - [PentagonIndexCount](#P-H3Lib-H3Index-PentagonIndexCount 'H3Lib.H3Index.PentagonIndexCount')
  - [ReservedBits](#P-H3Lib-H3Index-ReservedBits 'H3Lib.H3Index.ReservedBits')
  - [Resolution](#P-H3Lib-H3Index-Resolution 'H3Lib.H3Index.Resolution')
  - [CompareTo()](#M-H3Lib-H3Index-CompareTo-H3Lib-H3Index- 'H3Lib.H3Index.CompareTo(H3Lib.H3Index)')
  - [Equals(other)](#M-H3Lib-H3Index-Equals-H3Lib-H3Index- 'H3Lib.H3Index.Equals(H3Lib.H3Index)')
  - [Equals()](#M-H3Lib-H3Index-Equals-System-UInt64- 'H3Lib.H3Index.Equals(System.UInt64)')
  - [Equals()](#M-H3Lib-H3Index-Equals-System-Object- 'H3Lib.H3Index.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-H3Index-GetHashCode 'H3Lib.H3Index.GetHashCode')
  - [GetIndexDigit()](#M-H3Lib-H3Index-GetIndexDigit-System-Int32- 'H3Lib.H3Index.GetIndexDigit(System.Int32)')
  - [ToString()](#M-H3Lib-H3Index-ToString 'H3Lib.H3Index.ToString')
  - [op_Equality()](#M-H3Lib-H3Index-op_Equality-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.H3Index.op_Equality(H3Lib.H3Index,H3Lib.H3Index)')
  - [op_Implicit()](#M-H3Lib-H3Index-op_Implicit-System-UInt64-~H3Lib-H3Index 'H3Lib.H3Index.op_Implicit(System.UInt64)~H3Lib.H3Index')
  - [op_Implicit()](#M-H3Lib-H3Index-op_Implicit-H3Lib-H3Index-~System-UInt64 'H3Lib.H3Index.op_Implicit(H3Lib.H3Index)~System.UInt64')
  - [op_Inequality()](#M-H3Lib-H3Index-op_Inequality-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.H3Index.op_Inequality(H3Lib.H3Index,H3Lib.H3Index)')
- [H3IndexExtensions](#T-H3Lib-Extensions-H3IndexExtensions 'H3Lib.Extensions.H3IndexExtensions')
  - [CellAreaKm2(h)](#M-H3Lib-Extensions-H3IndexExtensions-CellAreaKm2-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.CellAreaKm2(H3Lib.H3Index)')
  - [CellAreaM2(h)](#M-H3Lib-Extensions-H3IndexExtensions-CellAreaM2-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.CellAreaM2(H3Lib.H3Index)')
  - [CellAreaRadians2(cell)](#M-H3Lib-Extensions-H3IndexExtensions-CellAreaRadians2-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.CellAreaRadians2(H3Lib.H3Index)')
  - [DestinationFromUniDirectionalEdge(edge)](#M-H3Lib-Extensions-H3IndexExtensions-DestinationFromUniDirectionalEdge-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.DestinationFromUniDirectionalEdge(H3Lib.H3Index)')
  - [DistanceTo(origin,h3)](#M-H3Lib-Extensions-H3IndexExtensions-DistanceTo-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.DistanceTo(H3Lib.H3Index,H3Lib.H3Index)')
  - [ExactEdgeLengthKm(edge)](#M-H3Lib-Extensions-H3IndexExtensions-ExactEdgeLengthKm-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ExactEdgeLengthKm(H3Lib.H3Index)')
  - [ExactEdgeLengthM(edge)](#M-H3Lib-Extensions-H3IndexExtensions-ExactEdgeLengthM-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ExactEdgeLengthM(H3Lib.H3Index)')
  - [ExactEdgeLengthRads(edge)](#M-H3Lib-Extensions-H3IndexExtensions-ExactEdgeLengthRads-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ExactEdgeLengthRads(H3Lib.H3Index)')
  - [GetFaces(h3)](#M-H3Lib-Extensions-H3IndexExtensions-GetFaces-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.GetFaces(H3Lib.H3Index)')
  - [GetH3IndexesArrayFromUniEdge(edge)](#M-H3Lib-Extensions-H3IndexExtensions-GetH3IndexesArrayFromUniEdge-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.GetH3IndexesArrayFromUniEdge(H3Lib.H3Index)')
  - [GetH3IndexesFromUniEdge(edge)](#M-H3Lib-Extensions-H3IndexExtensions-GetH3IndexesFromUniEdge-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.GetH3IndexesFromUniEdge(H3Lib.H3Index)')
  - [GetUniEdgesFromCell(origin)](#M-H3Lib-Extensions-H3IndexExtensions-GetUniEdgesFromCell-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.GetUniEdgesFromCell(H3Lib.H3Index)')
  - [HexRadiusKm(h3)](#M-H3Lib-Extensions-H3IndexExtensions-HexRadiusKm-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.HexRadiusKm(H3Lib.H3Index)')
  - [HexRange(origin,k)](#M-H3Lib-Extensions-H3IndexExtensions-HexRange-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.HexRange(H3Lib.H3Index,System.Int32)')
  - [HexRangeDistances(origin,k)](#M-H3Lib-Extensions-H3IndexExtensions-HexRangeDistances-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.HexRangeDistances(H3Lib.H3Index,System.Int32)')
  - [HexRing(origin,k)](#M-H3Lib-Extensions-H3IndexExtensions-HexRing-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.HexRing(H3Lib.H3Index,System.Int32)')
  - [IsNeighborTo(origin,destination)](#M-H3Lib-Extensions-H3IndexExtensions-IsNeighborTo-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.IsNeighborTo(H3Lib.H3Index,H3Lib.H3Index)')
  - [IsPentagon(h)](#M-H3Lib-Extensions-H3IndexExtensions-IsPentagon-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.IsPentagon(H3Lib.H3Index)')
  - [IsValid(h)](#M-H3Lib-Extensions-H3IndexExtensions-IsValid-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.IsValid(H3Lib.H3Index)')
  - [IsValidUniEdge(edge)](#M-H3Lib-Extensions-H3IndexExtensions-IsValidUniEdge-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.IsValidUniEdge(H3Lib.H3Index)')
  - [KRing(origin,k)](#M-H3Lib-Extensions-H3IndexExtensions-KRing-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.KRing(H3Lib.H3Index,System.Int32)')
  - [KRingDistances(origin,k)](#M-H3Lib-Extensions-H3IndexExtensions-KRingDistances-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.KRingDistances(H3Lib.H3Index,System.Int32)')
  - [KRingInternal(origin,k,currentK,outData)](#M-H3Lib-Extensions-H3IndexExtensions-KRingInternal-H3Lib-H3Index,System-Int32,System-Int32,System-Collections-Generic-Dictionary{H3Lib-H3Index,System-Int32}- 'H3Lib.Extensions.H3IndexExtensions.KRingInternal(H3Lib.H3Index,System.Int32,System.Int32,System.Collections.Generic.Dictionary{H3Lib.H3Index,System.Int32})')
  - [LineSize(start,end)](#M-H3Lib-Extensions-H3IndexExtensions-LineSize-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.LineSize(H3Lib.H3Index,H3Lib.H3Index)')
  - [LineTo(start,end)](#M-H3Lib-Extensions-H3IndexExtensions-LineTo-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.LineTo(H3Lib.H3Index,H3Lib.H3Index)')
  - [MakeDirectChild(h,cellNumber)](#M-H3Lib-Extensions-H3IndexExtensions-MakeDirectChild-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.MakeDirectChild(H3Lib.H3Index,System.Int32)')
  - [MaxChildrenSize(h3,childRes)](#M-H3Lib-Extensions-H3IndexExtensions-MaxChildrenSize-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.MaxChildrenSize(H3Lib.H3Index,System.Int32)')
  - [MaxFaceCount(h3)](#M-H3Lib-Extensions-H3IndexExtensions-MaxFaceCount-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.MaxFaceCount(H3Lib.H3Index)')
  - [MaxUncompactSize(singleCell,res)](#M-H3Lib-Extensions-H3IndexExtensions-MaxUncompactSize-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.MaxUncompactSize(H3Lib.H3Index,System.Int32)')
  - [NeighborRotations(origin,dir,rotations)](#M-H3Lib-Extensions-H3IndexExtensions-NeighborRotations-H3Lib-H3Index,H3Lib-Direction,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.NeighborRotations(H3Lib.H3Index,H3Lib.Direction,System.Int32)')
  - [OriginFromUniDirectionalEdge(edge)](#M-H3Lib-Extensions-H3IndexExtensions-OriginFromUniDirectionalEdge-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.OriginFromUniDirectionalEdge(H3Lib.H3Index)')
  - [Rotate60Clockwise(h)](#M-H3Lib-Extensions-H3IndexExtensions-Rotate60Clockwise-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.Rotate60Clockwise(H3Lib.H3Index)')
  - [Rotate60CounterClockwise(h)](#M-H3Lib-Extensions-H3IndexExtensions-Rotate60CounterClockwise-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.Rotate60CounterClockwise(H3Lib.H3Index)')
  - [RotatePent60Clockwise(h)](#M-H3Lib-Extensions-H3IndexExtensions-RotatePent60Clockwise-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.RotatePent60Clockwise(H3Lib.H3Index)')
  - [RotatePent60CounterClockwise(h)](#M-H3Lib-Extensions-H3IndexExtensions-RotatePent60CounterClockwise-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.RotatePent60CounterClockwise(H3Lib.H3Index)')
  - [SetBaseCell()](#M-H3Lib-Extensions-H3IndexExtensions-SetBaseCell-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.SetBaseCell(H3Lib.H3Index,System.Int32)')
  - [SetHighBit()](#M-H3Lib-Extensions-H3IndexExtensions-SetHighBit-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.SetHighBit(H3Lib.H3Index,System.Int32)')
  - [SetIndex(hp,res,baseCell,initDigit)](#M-H3Lib-Extensions-H3IndexExtensions-SetIndex-H3Lib-H3Index,System-Int32,System-Int32,H3Lib-Direction- 'H3Lib.Extensions.H3IndexExtensions.SetIndex(H3Lib.H3Index,System.Int32,System.Int32,H3Lib.Direction)')
  - [SetIndexDigit()](#M-H3Lib-Extensions-H3IndexExtensions-SetIndexDigit-H3Lib-H3Index,System-Int32,System-UInt64- 'H3Lib.Extensions.H3IndexExtensions.SetIndexDigit(H3Lib.H3Index,System.Int32,System.UInt64)')
  - [SetMode()](#M-H3Lib-Extensions-H3IndexExtensions-SetMode-H3Lib-H3Index,H3Lib-H3Mode- 'H3Lib.Extensions.H3IndexExtensions.SetMode(H3Lib.H3Index,H3Lib.H3Mode)')
  - [SetReservedBits()](#M-H3Lib-Extensions-H3IndexExtensions-SetReservedBits-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.SetReservedBits(H3Lib.H3Index,System.Int32)')
  - [SetResolution()](#M-H3Lib-Extensions-H3IndexExtensions-SetResolution-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.SetResolution(H3Lib.H3Index,System.Int32)')
  - [ToCenterChild(h,childRes)](#M-H3Lib-Extensions-H3IndexExtensions-ToCenterChild-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.ToCenterChild(H3Lib.H3Index,System.Int32)')
  - [ToChildren(h,childRes)](#M-H3Lib-Extensions-H3IndexExtensions-ToChildren-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.ToChildren(H3Lib.H3Index,System.Int32)')
  - [ToFaceIjk(h)](#M-H3Lib-Extensions-H3IndexExtensions-ToFaceIjk-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ToFaceIjk(H3Lib.H3Index)')
  - [ToFaceIjkWithInitializedFijk(h,fijk)](#M-H3Lib-Extensions-H3IndexExtensions-ToFaceIjkWithInitializedFijk-H3Lib-H3Index,H3Lib-FaceIjk- 'H3Lib.Extensions.H3IndexExtensions.ToFaceIjkWithInitializedFijk(H3Lib.H3Index,H3Lib.FaceIjk)')
  - [ToGeoBoundary(h3)](#M-H3Lib-Extensions-H3IndexExtensions-ToGeoBoundary-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ToGeoBoundary(H3Lib.H3Index)')
  - [ToGeoCoord(h3)](#M-H3Lib-Extensions-H3IndexExtensions-ToGeoCoord-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ToGeoCoord(H3Lib.H3Index)')
  - [ToLocalIjExperimental(origin,h3)](#M-H3Lib-Extensions-H3IndexExtensions-ToLocalIjExperimental-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ToLocalIjExperimental(H3Lib.H3Index,H3Lib.H3Index)')
  - [ToLocalIjk(origin,h3)](#M-H3Lib-Extensions-H3IndexExtensions-ToLocalIjk-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.ToLocalIjk(H3Lib.H3Index,H3Lib.H3Index)')
  - [ToParent(h,parentRes)](#M-H3Lib-Extensions-H3IndexExtensions-ToParent-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.ToParent(H3Lib.H3Index,System.Int32)')
  - [Uncompact(singleCell,res)](#M-H3Lib-Extensions-H3IndexExtensions-Uncompact-H3Lib-H3Index,System-Int32- 'H3Lib.Extensions.H3IndexExtensions.Uncompact(H3Lib.H3Index,System.Int32)')
  - [UniDirectionalEdgeTo(origin,destination)](#M-H3Lib-Extensions-H3IndexExtensions-UniDirectionalEdgeTo-H3Lib-H3Index,H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.UniDirectionalEdgeTo(H3Lib.H3Index,H3Lib.H3Index)')
  - [UniEdgeToGeoBoundary(edge)](#M-H3Lib-Extensions-H3IndexExtensions-UniEdgeToGeoBoundary-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.UniEdgeToGeoBoundary(H3Lib.H3Index)')
  - [VertexNumForDirection()](#M-H3Lib-Extensions-H3IndexExtensions-VertexNumForDirection-H3Lib-H3Index,H3Lib-Direction- 'H3Lib.Extensions.H3IndexExtensions.VertexNumForDirection(H3Lib.H3Index,H3Lib.Direction)')
  - [VertexRotations()](#M-H3Lib-Extensions-H3IndexExtensions-VertexRotations-H3Lib-H3Index- 'H3Lib.Extensions.H3IndexExtensions.VertexRotations(H3Lib.H3Index)')
- [H3LibExtensions](#T-H3Lib-Extensions-H3LibExtensions 'H3Lib.Extensions.H3LibExtensions')
  - [Compact(h3Set)](#M-H3Lib-Extensions-H3LibExtensions-Compact-System-Collections-Generic-List{H3Lib-H3Index}- 'H3Lib.Extensions.H3LibExtensions.Compact(System.Collections.Generic.List{H3Lib.H3Index})')
  - [ConstrainLatitude(latitude)](#M-H3Lib-Extensions-H3LibExtensions-ConstrainLatitude-System-Double- 'H3Lib.Extensions.H3LibExtensions.ConstrainLatitude(System.Double)')
  - [ConstrainLatitude()](#M-H3Lib-Extensions-H3LibExtensions-ConstrainLatitude-System-Int32- 'H3Lib.Extensions.H3LibExtensions.ConstrainLatitude(System.Int32)')
  - [ConstrainLongitude(longitude)](#M-H3Lib-Extensions-H3LibExtensions-ConstrainLongitude-System-Double- 'H3Lib.Extensions.H3LibExtensions.ConstrainLongitude(System.Double)')
  - [ConstrainLongitude(longitude)](#M-H3Lib-Extensions-H3LibExtensions-ConstrainLongitude-System-Int32- 'H3Lib.Extensions.H3LibExtensions.ConstrainLongitude(System.Int32)')
  - [DegreesToRadians(degrees)](#M-H3Lib-Extensions-H3LibExtensions-DegreesToRadians-System-Double- 'H3Lib.Extensions.H3LibExtensions.DegreesToRadians(System.Double)')
  - [DegreesToRadians(degrees)](#M-H3Lib-Extensions-H3LibExtensions-DegreesToRadians-System-Int32- 'H3Lib.Extensions.H3LibExtensions.DegreesToRadians(System.Int32)')
  - [FlexiCompact()](#M-H3Lib-Extensions-H3LibExtensions-FlexiCompact-System-Collections-Generic-List{H3Lib-H3Index}- 'H3Lib.Extensions.H3LibExtensions.FlexiCompact(System.Collections.Generic.List{H3Lib.H3Index})')
  - [GetPentagonIndexes(res)](#M-H3Lib-Extensions-H3LibExtensions-GetPentagonIndexes-System-Int32- 'H3Lib.Extensions.H3LibExtensions.GetPentagonIndexes(System.Int32)')
  - [IsResClassIii(res)](#M-H3Lib-Extensions-H3LibExtensions-IsResClassIii-System-Int32- 'H3Lib.Extensions.H3LibExtensions.IsResClassIii(System.Int32)')
  - [IsValidChildRes(parentRes,childRes)](#M-H3Lib-Extensions-H3LibExtensions-IsValidChildRes-System-Int32,System-Int32- 'H3Lib.Extensions.H3LibExtensions.IsValidChildRes(System.Int32,System.Int32)')
  - [MaxKringSize(k)](#M-H3Lib-Extensions-H3LibExtensions-MaxKringSize-System-Int32- 'H3Lib.Extensions.H3LibExtensions.MaxKringSize(System.Int32)')
  - [NormalizeLongitude()](#M-H3Lib-Extensions-H3LibExtensions-NormalizeLongitude-System-Double,System-Boolean- 'H3Lib.Extensions.H3LibExtensions.NormalizeLongitude(System.Double,System.Boolean)')
  - [NormalizeRadians(rads,limit)](#M-H3Lib-Extensions-H3LibExtensions-NormalizeRadians-System-Double,System-Double- 'H3Lib.Extensions.H3LibExtensions.NormalizeRadians(System.Double,System.Double)')
  - [NumHexagons(res)](#M-H3Lib-Extensions-H3LibExtensions-NumHexagons-System-Int32- 'H3Lib.Extensions.H3LibExtensions.NumHexagons(System.Int32)')
  - [Power(baseValue,power)](#M-H3Lib-Extensions-H3LibExtensions-Power-System-Int64,System-Int64- 'H3Lib.Extensions.H3LibExtensions.Power(System.Int64,System.Int64)')
  - [RadiansToDegrees(radians)](#M-H3Lib-Extensions-H3LibExtensions-RadiansToDegrees-System-Double- 'H3Lib.Extensions.H3LibExtensions.RadiansToDegrees(System.Double)')
  - [Square(x)](#M-H3Lib-Extensions-H3LibExtensions-Square-System-Double- 'H3Lib.Extensions.H3LibExtensions.Square(System.Double)')
  - [ToH3Index(s)](#M-H3Lib-Extensions-H3LibExtensions-ToH3Index-System-String- 'H3Lib.Extensions.H3LibExtensions.ToH3Index(System.String)')
- [H3Mode](#T-H3Lib-H3Mode 'H3Lib.H3Mode')
  - [Hexagon](#F-H3Lib-H3Mode-Hexagon 'H3Lib.H3Mode.Hexagon')
  - [UniEdge](#F-H3Lib-H3Mode-UniEdge 'H3Lib.H3Mode.UniEdge')
- [LinkedGeoCoord](#T-H3Lib-LinkedGeoCoord 'H3Lib.LinkedGeoCoord')
  - [#ctor()](#M-H3Lib-LinkedGeoCoord-#ctor 'H3Lib.LinkedGeoCoord.#ctor')
  - [#ctor()](#M-H3Lib-LinkedGeoCoord-#ctor-H3Lib-GeoCoord- 'H3Lib.LinkedGeoCoord.#ctor(H3Lib.GeoCoord)')
  - [_gc](#F-H3Lib-LinkedGeoCoord-_gc 'H3Lib.LinkedGeoCoord._gc')
  - [Latitude](#P-H3Lib-LinkedGeoCoord-Latitude 'H3Lib.LinkedGeoCoord.Latitude')
  - [Longitude](#P-H3Lib-LinkedGeoCoord-Longitude 'H3Lib.LinkedGeoCoord.Longitude')
  - [Vertex](#P-H3Lib-LinkedGeoCoord-Vertex 'H3Lib.LinkedGeoCoord.Vertex')
  - [Replacement()](#M-H3Lib-LinkedGeoCoord-Replacement-H3Lib-GeoCoord- 'H3Lib.LinkedGeoCoord.Replacement(H3Lib.GeoCoord)')
- [LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop')
  - [#ctor()](#M-H3Lib-LinkedGeoLoop-#ctor 'H3Lib.LinkedGeoLoop.#ctor')
  - [Loop](#F-H3Lib-LinkedGeoLoop-Loop 'H3Lib.LinkedGeoLoop.Loop')
  - [Count](#P-H3Lib-LinkedGeoLoop-Count 'H3Lib.LinkedGeoLoop.Count')
  - [First](#P-H3Lib-LinkedGeoLoop-First 'H3Lib.LinkedGeoLoop.First')
  - [IsEmpty](#P-H3Lib-LinkedGeoLoop-IsEmpty 'H3Lib.LinkedGeoLoop.IsEmpty')
  - [Nodes](#P-H3Lib-LinkedGeoLoop-Nodes 'H3Lib.LinkedGeoLoop.Nodes')
  - [AddLinkedCoord(vertex)](#M-H3Lib-LinkedGeoLoop-AddLinkedCoord-H3Lib-GeoCoord- 'H3Lib.LinkedGeoLoop.AddLinkedCoord(H3Lib.GeoCoord)')
  - [Clear()](#M-H3Lib-LinkedGeoLoop-Clear 'H3Lib.LinkedGeoLoop.Clear')
  - [CopyNodes()](#M-H3Lib-LinkedGeoLoop-CopyNodes 'H3Lib.LinkedGeoLoop.CopyNodes')
  - [Destroy()](#M-H3Lib-LinkedGeoLoop-Destroy 'H3Lib.LinkedGeoLoop.Destroy')
  - [GetFirst()](#M-H3Lib-LinkedGeoLoop-GetFirst 'H3Lib.LinkedGeoLoop.GetFirst')
- [LinkedGeoLoopExtensions](#T-H3Lib-Extensions-LinkedGeoLoopExtensions 'H3Lib.Extensions.LinkedGeoLoopExtensions')
  - [CountContainers(loop,polygons,boxes)](#M-H3Lib-Extensions-LinkedGeoLoopExtensions-CountContainers-H3Lib-LinkedGeoLoop,System-Collections-Generic-List{H3Lib-LinkedGeoPolygon},System-Collections-Generic-List{H3Lib-BBox}- 'H3Lib.Extensions.LinkedGeoLoopExtensions.CountContainers(H3Lib.LinkedGeoLoop,System.Collections.Generic.List{H3Lib.LinkedGeoPolygon},System.Collections.Generic.List{H3Lib.BBox})')
  - [IsClockwise(loop)](#M-H3Lib-Extensions-LinkedGeoLoopExtensions-IsClockwise-H3Lib-LinkedGeoLoop- 'H3Lib.Extensions.LinkedGeoLoopExtensions.IsClockwise(H3Lib.LinkedGeoLoop)')
  - [IsClockwiseNormalized(loop,isTransmeridian)](#M-H3Lib-Extensions-LinkedGeoLoopExtensions-IsClockwiseNormalized-H3Lib-LinkedGeoLoop,System-Boolean- 'H3Lib.Extensions.LinkedGeoLoopExtensions.IsClockwiseNormalized(H3Lib.LinkedGeoLoop,System.Boolean)')
  - [PointInside(loop,box,coord)](#M-H3Lib-Extensions-LinkedGeoLoopExtensions-PointInside-H3Lib-LinkedGeoLoop,H3Lib-BBox,H3Lib-GeoCoord- 'H3Lib.Extensions.LinkedGeoLoopExtensions.PointInside(H3Lib.LinkedGeoLoop,H3Lib.BBox,H3Lib.GeoCoord)')
  - [ToBBox(loop)](#M-H3Lib-Extensions-LinkedGeoLoopExtensions-ToBBox-H3Lib-LinkedGeoLoop- 'H3Lib.Extensions.LinkedGeoLoopExtensions.ToBBox(H3Lib.LinkedGeoLoop)')
- [LinkedGeoPolygon](#T-H3Lib-LinkedGeoPolygon 'H3Lib.LinkedGeoPolygon')
  - [#ctor()](#M-H3Lib-LinkedGeoPolygon-#ctor 'H3Lib.LinkedGeoPolygon.#ctor')
  - [Next](#F-H3Lib-LinkedGeoPolygon-Next 'H3Lib.LinkedGeoPolygon.Next')
  - [_geoLoops](#F-H3Lib-LinkedGeoPolygon-_geoLoops 'H3Lib.LinkedGeoPolygon._geoLoops')
  - [CountLoops](#P-H3Lib-LinkedGeoPolygon-CountLoops 'H3Lib.LinkedGeoPolygon.CountLoops')
  - [CountPolygons](#P-H3Lib-LinkedGeoPolygon-CountPolygons 'H3Lib.LinkedGeoPolygon.CountPolygons')
  - [First](#P-H3Lib-LinkedGeoPolygon-First 'H3Lib.LinkedGeoPolygon.First')
  - [Last](#P-H3Lib-LinkedGeoPolygon-Last 'H3Lib.LinkedGeoPolygon.Last')
  - [LinkedPolygons](#P-H3Lib-LinkedGeoPolygon-LinkedPolygons 'H3Lib.LinkedGeoPolygon.LinkedPolygons')
  - [Loops](#P-H3Lib-LinkedGeoPolygon-Loops 'H3Lib.LinkedGeoPolygon.Loops')
  - [AddLinkedLoop(loop)](#M-H3Lib-LinkedGeoPolygon-AddLinkedLoop-H3Lib-LinkedGeoLoop- 'H3Lib.LinkedGeoPolygon.AddLinkedLoop(H3Lib.LinkedGeoLoop)')
  - [AddNewLinkedGeoPolygon()](#M-H3Lib-LinkedGeoPolygon-AddNewLinkedGeoPolygon 'H3Lib.LinkedGeoPolygon.AddNewLinkedGeoPolygon')
  - [AddNewLinkedLoop()](#M-H3Lib-LinkedGeoPolygon-AddNewLinkedLoop 'H3Lib.LinkedGeoPolygon.AddNewLinkedLoop')
  - [Clear()](#M-H3Lib-LinkedGeoPolygon-Clear 'H3Lib.LinkedGeoPolygon.Clear')
  - [Destroy()](#M-H3Lib-LinkedGeoPolygon-Destroy 'H3Lib.LinkedGeoPolygon.Destroy')
  - [GetFirst()](#M-H3Lib-LinkedGeoPolygon-GetFirst 'H3Lib.LinkedGeoPolygon.GetFirst')
  - [GetLast()](#M-H3Lib-LinkedGeoPolygon-GetLast 'H3Lib.LinkedGeoPolygon.GetLast')
  - [GetPolygons()](#M-H3Lib-LinkedGeoPolygon-GetPolygons 'H3Lib.LinkedGeoPolygon.GetPolygons')
  - [TotalPolygons()](#M-H3Lib-LinkedGeoPolygon-TotalPolygons 'H3Lib.LinkedGeoPolygon.TotalPolygons')
- [LinkedGeoPolygonExtensions](#T-H3Lib-Extensions-LinkedGeoPolygonExtensions 'H3Lib.Extensions.LinkedGeoPolygonExtensions')
  - [FindPolygonForHole(loop,polygon,boxes,polygonCount)](#M-H3Lib-Extensions-LinkedGeoPolygonExtensions-FindPolygonForHole-H3Lib-LinkedGeoLoop,H3Lib-LinkedGeoPolygon,System-Collections-Generic-List{H3Lib-BBox},System-Int32- 'H3Lib.Extensions.LinkedGeoPolygonExtensions.FindPolygonForHole(H3Lib.LinkedGeoLoop,H3Lib.LinkedGeoPolygon,System.Collections.Generic.List{H3Lib.BBox},System.Int32)')
  - [NormalizeMultiPolygon(root)](#M-H3Lib-Extensions-LinkedGeoPolygonExtensions-NormalizeMultiPolygon-H3Lib-LinkedGeoPolygon- 'H3Lib.Extensions.LinkedGeoPolygonExtensions.NormalizeMultiPolygon(H3Lib.LinkedGeoPolygon)')
- [LocalIJ](#T-H3Lib-Constants-LocalIJ 'H3Lib.Constants.LocalIJ')
  - [FAILED_DIRECTIONS](#F-H3Lib-Constants-LocalIJ-FAILED_DIRECTIONS 'H3Lib.Constants.LocalIJ.FAILED_DIRECTIONS')
  - [PENTAGON_ROTATIONS](#F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS 'H3Lib.Constants.LocalIJ.PENTAGON_ROTATIONS')
  - [PENTAGON_ROTATIONS_REVERSE](#F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS_REVERSE 'H3Lib.Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE')
  - [PENTAGON_ROTATIONS_REVERSE_NONPOLAR](#F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS_REVERSE_NONPOLAR 'H3Lib.Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE_NONPOLAR')
  - [PENTAGON_ROTATIONS_REVERSE_POLAR](#F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS_REVERSE_POLAR 'H3Lib.Constants.LocalIJ.PENTAGON_ROTATIONS_REVERSE_POLAR')
- [Overage](#T-H3Lib-Overage 'H3Lib.Overage')
  - [FACE_EDGE](#F-H3Lib-Overage-FACE_EDGE 'H3Lib.Overage.FACE_EDGE')
  - [NEW_FACE](#F-H3Lib-Overage-NEW_FACE 'H3Lib.Overage.NEW_FACE')
  - [NO_OVERAGE](#F-H3Lib-Overage-NO_OVERAGE 'H3Lib.Overage.NO_OVERAGE')
- [PentagonDirectionFace](#T-H3Lib-PentagonDirectionFace 'H3Lib.PentagonDirectionFace')
  - [#ctor(bc,faces)](#M-H3Lib-PentagonDirectionFace-#ctor-System-Int32,System-Collections-Generic-IList{System-Int32}- 'H3Lib.PentagonDirectionFace.#ctor(System.Int32,System.Collections.Generic.IList{System.Int32})')
  - [#ctor(raw)](#M-H3Lib-PentagonDirectionFace-#ctor-System-Collections-Generic-IList{System-Int32}- 'H3Lib.PentagonDirectionFace.#ctor(System.Collections.Generic.IList{System.Int32})')
  - [#ctor()](#M-H3Lib-PentagonDirectionFace-#ctor-System-Int32,System-Int32,System-Int32,System-Int32,System-Int32,System-Int32- 'H3Lib.PentagonDirectionFace.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)')
  - [BaseCell](#F-H3Lib-PentagonDirectionFace-BaseCell 'H3Lib.PentagonDirectionFace.BaseCell')
  - [Faces](#F-H3Lib-PentagonDirectionFace-Faces 'H3Lib.PentagonDirectionFace.Faces')
- [Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d')
  - [#ctor()](#M-H3Lib-Vec2d-#ctor-System-Double,System-Double- 'H3Lib.Vec2d.#ctor(System.Double,System.Double)')
  - [X](#F-H3Lib-Vec2d-X 'H3Lib.Vec2d.X')
  - [Y](#F-H3Lib-Vec2d-Y 'H3Lib.Vec2d.Y')
  - [Magnitude](#P-H3Lib-Vec2d-Magnitude 'H3Lib.Vec2d.Magnitude')
  - [Equals()](#M-H3Lib-Vec2d-Equals-H3Lib-Vec2d- 'H3Lib.Vec2d.Equals(H3Lib.Vec2d)')
  - [Equals()](#M-H3Lib-Vec2d-Equals-System-Object- 'H3Lib.Vec2d.Equals(System.Object)')
  - [FindIntersection(p0,p1,p2,p3)](#M-H3Lib-Vec2d-FindIntersection-H3Lib-Vec2d,H3Lib-Vec2d,H3Lib-Vec2d,H3Lib-Vec2d- 'H3Lib.Vec2d.FindIntersection(H3Lib.Vec2d,H3Lib.Vec2d,H3Lib.Vec2d,H3Lib.Vec2d)')
  - [GetHashCode()](#M-H3Lib-Vec2d-GetHashCode 'H3Lib.Vec2d.GetHashCode')
  - [ToString()](#M-H3Lib-Vec2d-ToString 'H3Lib.Vec2d.ToString')
  - [op_Equality()](#M-H3Lib-Vec2d-op_Equality-H3Lib-Vec2d,H3Lib-Vec2d- 'H3Lib.Vec2d.op_Equality(H3Lib.Vec2d,H3Lib.Vec2d)')
  - [op_Inequality()](#M-H3Lib-Vec2d-op_Inequality-H3Lib-Vec2d,H3Lib-Vec2d- 'H3Lib.Vec2d.op_Inequality(H3Lib.Vec2d,H3Lib.Vec2d)')
- [Vec2dExtensions](#T-H3Lib-Extensions-Vec2dExtensions 'H3Lib.Extensions.Vec2dExtensions')
  - [ToCoordIjk(v)](#M-H3Lib-Extensions-Vec2dExtensions-ToCoordIjk-H3Lib-Vec2d- 'H3Lib.Extensions.Vec2dExtensions.ToCoordIjk(H3Lib.Vec2d)')
  - [ToGeoCoord(v,face,res,substrate)](#M-H3Lib-Extensions-Vec2dExtensions-ToGeoCoord-H3Lib-Vec2d,System-Int32,System-Int32,System-Int32- 'H3Lib.Extensions.Vec2dExtensions.ToGeoCoord(H3Lib.Vec2d,System.Int32,System.Int32,System.Int32)')
- [Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d')
  - [#ctor()](#M-H3Lib-Vec3d-#ctor-System-Double,System-Double,System-Double- 'H3Lib.Vec3d.#ctor(System.Double,System.Double,System.Double)')
  - [X](#F-H3Lib-Vec3d-X 'H3Lib.Vec3d.X')
  - [Y](#F-H3Lib-Vec3d-Y 'H3Lib.Vec3d.Y')
  - [Z](#F-H3Lib-Vec3d-Z 'H3Lib.Vec3d.Z')
  - [Equals()](#M-H3Lib-Vec3d-Equals-H3Lib-Vec3d- 'H3Lib.Vec3d.Equals(H3Lib.Vec3d)')
  - [Equals()](#M-H3Lib-Vec3d-Equals-System-Object- 'H3Lib.Vec3d.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-Vec3d-GetHashCode 'H3Lib.Vec3d.GetHashCode')
  - [ToString()](#M-H3Lib-Vec3d-ToString 'H3Lib.Vec3d.ToString')
  - [op_Equality(left,right)](#M-H3Lib-Vec3d-op_Equality-H3Lib-Vec3d,H3Lib-Vec3d- 'H3Lib.Vec3d.op_Equality(H3Lib.Vec3d,H3Lib.Vec3d)')
  - [op_Inequality()](#M-H3Lib-Vec3d-op_Inequality-H3Lib-Vec3d,H3Lib-Vec3d- 'H3Lib.Vec3d.op_Inequality(H3Lib.Vec3d,H3Lib.Vec3d)')
- [Vec3dExtensions](#T-H3Lib-Extensions-Vec3dExtensions 'H3Lib.Extensions.Vec3dExtensions')
  - [PointSquareDistance(v1,v2)](#M-H3Lib-Extensions-Vec3dExtensions-PointSquareDistance-H3Lib-Vec3d,H3Lib-Vec3d- 'H3Lib.Extensions.Vec3dExtensions.PointSquareDistance(H3Lib.Vec3d,H3Lib.Vec3d)')
  - [SetX(v3,x)](#M-H3Lib-Extensions-Vec3dExtensions-SetX-H3Lib-Vec3d,System-Double- 'H3Lib.Extensions.Vec3dExtensions.SetX(H3Lib.Vec3d,System.Double)')
  - [SetY(v3,y)](#M-H3Lib-Extensions-Vec3dExtensions-SetY-H3Lib-Vec3d,System-Double- 'H3Lib.Extensions.Vec3dExtensions.SetY(H3Lib.Vec3d,System.Double)')
  - [SetZ(v3,z)](#M-H3Lib-Extensions-Vec3dExtensions-SetZ-H3Lib-Vec3d,System-Double- 'H3Lib.Extensions.Vec3dExtensions.SetZ(H3Lib.Vec3d,System.Double)')
- [Vertex](#T-H3Lib-Constants-Vertex 'H3Lib.Constants.Vertex')
  - [DirectionToVertexNumHex](#F-H3Lib-Constants-Vertex-DirectionToVertexNumHex 'H3Lib.Constants.Vertex.DirectionToVertexNumHex')
  - [DirectionToVertexNumPent](#F-H3Lib-Constants-Vertex-DirectionToVertexNumPent 'H3Lib.Constants.Vertex.DirectionToVertexNumPent')
  - [INVALID_VERTEX_NUM](#F-H3Lib-Constants-Vertex-INVALID_VERTEX_NUM 'H3Lib.Constants.Vertex.INVALID_VERTEX_NUM')
  - [MAX_BASE_CELL_FACES](#F-H3Lib-Constants-Vertex-MAX_BASE_CELL_FACES 'H3Lib.Constants.Vertex.MAX_BASE_CELL_FACES')
  - [PentagonDirectionFaces](#F-H3Lib-Constants-Vertex-PentagonDirectionFaces 'H3Lib.Constants.Vertex.PentagonDirectionFaces')
- [VertexGraph](#T-H3Lib-VertexGraph 'H3Lib.VertexGraph')
  - [#ctor()](#M-H3Lib-VertexGraph-#ctor 'H3Lib.VertexGraph.#ctor')
  - [#ctor(res)](#M-H3Lib-VertexGraph-#ctor-System-Int32- 'H3Lib.VertexGraph.#ctor(System.Int32)')
  - [Resolution](#F-H3Lib-VertexGraph-Resolution 'H3Lib.VertexGraph.Resolution')
  - [_pool](#F-H3Lib-VertexGraph-_pool 'H3Lib.VertexGraph._pool')
  - [Count](#P-H3Lib-VertexGraph-Count 'H3Lib.VertexGraph.Count')
  - [Size](#P-H3Lib-VertexGraph-Size 'H3Lib.VertexGraph.Size')
  - [AddNode(fromNode,toNode)](#M-H3Lib-VertexGraph-AddNode-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.VertexGraph.AddNode(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [Clear()](#M-H3Lib-VertexGraph-Clear 'H3Lib.VertexGraph.Clear')
  - [FindEdge(fromNode,toNode)](#M-H3Lib-VertexGraph-FindEdge-H3Lib-GeoCoord,System-Nullable{H3Lib-GeoCoord}- 'H3Lib.VertexGraph.FindEdge(H3Lib.GeoCoord,System.Nullable{H3Lib.GeoCoord})')
  - [FindVertex(vertex)](#M-H3Lib-VertexGraph-FindVertex-H3Lib-GeoCoord- 'H3Lib.VertexGraph.FindVertex(H3Lib.GeoCoord)')
  - [FirstNode()](#M-H3Lib-VertexGraph-FirstNode 'H3Lib.VertexGraph.FirstNode')
  - [InitNode()](#M-H3Lib-VertexGraph-InitNode-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.VertexGraph.InitNode(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [RemoveNode(vn)](#M-H3Lib-VertexGraph-RemoveNode-System-Nullable{H3Lib-VertexNode}- 'H3Lib.VertexGraph.RemoveNode(System.Nullable{H3Lib.VertexNode})')
- [VertexGraphExtensions](#T-H3Lib-Extensions-VertexGraphExtensions 'H3Lib.Extensions.VertexGraphExtensions')
  - [ToLinkedGeoPolygon(graph)](#M-H3Lib-Extensions-VertexGraphExtensions-ToLinkedGeoPolygon-H3Lib-VertexGraph- 'H3Lib.Extensions.VertexGraphExtensions.ToLinkedGeoPolygon(H3Lib.VertexGraph)')
- [VertexNode](#T-H3Lib-VertexNode 'H3Lib.VertexNode')
  - [#ctor(toNode,fromNode)](#M-H3Lib-VertexNode-#ctor-H3Lib-GeoCoord,H3Lib-GeoCoord- 'H3Lib.VertexNode.#ctor(H3Lib.GeoCoord,H3Lib.GeoCoord)')
  - [From](#F-H3Lib-VertexNode-From 'H3Lib.VertexNode.From')
  - [To](#F-H3Lib-VertexNode-To 'H3Lib.VertexNode.To')
  - [Equals()](#M-H3Lib-VertexNode-Equals-H3Lib-VertexNode- 'H3Lib.VertexNode.Equals(H3Lib.VertexNode)')
  - [Equals()](#M-H3Lib-VertexNode-Equals-System-Object- 'H3Lib.VertexNode.Equals(System.Object)')
  - [GetHashCode()](#M-H3Lib-VertexNode-GetHashCode 'H3Lib.VertexNode.GetHashCode')
  - [op_Equality()](#M-H3Lib-VertexNode-op_Equality-H3Lib-VertexNode,H3Lib-VertexNode- 'H3Lib.VertexNode.op_Equality(H3Lib.VertexNode,H3Lib.VertexNode)')
  - [op_Inequality()](#M-H3Lib-VertexNode-op_Inequality-H3Lib-VertexNode,H3Lib-VertexNode- 'H3Lib.VertexNode.op_Inequality(H3Lib.VertexNode,H3Lib.VertexNode)')

<a name='T-H3Lib-Constants-Algos'></a>
## Algos `type`

##### Namespace

H3Lib.Constants

<a name='F-H3Lib-Constants-Algos-Directions'></a>
### Directions `constants`

##### Summary

```
     _
   _/ \_      Directions used for traversing a        
  / \5/ \     hexagonal ring counterclockwise
  \0/ \4/     around {1, 0, 0}
  / \_/ \
  \1/ \3/
    \2/
```

<a name='F-H3Lib-Constants-Algos-NewAdjustmentIi'></a>
### NewAdjustmentIi `constants`

##### Summary

New traversal direction when traversing along class II grids.

 Current digit -> direction -> new ap7 move (at coarser level).

<a name='F-H3Lib-Constants-Algos-NewAdjustmentIii'></a>
### NewAdjustmentIii `constants`

##### Summary

New traversal direction when traversing along class III grids.

 Current digit -gt; direction -gt; new ap7 move (at coarser level).

<a name='F-H3Lib-Constants-Algos-NewDigitIi'></a>
### NewDigitIi `constants`

##### Summary

New digit when traversing along class II grids.

 Current digit -> direction -> new digit.

<a name='F-H3Lib-Constants-Algos-NewDigitIii'></a>
### NewDigitIii `constants`

##### Summary

New traversal direction when traversing along class III grids.

 Current digit -> direction -> new ap7 move (at coarser level).

<a name='F-H3Lib-Constants-Algos-NextRingDirection'></a>
### NextRingDirection `constants`

##### Summary

Direction used for traversing to the next outward hexagonal ring.

<a name='T-H3Lib-Api'></a>
## Api `type`

##### Namespace

H3Lib

##### Summary

Primary H3 core library entry points.

<a name='M-H3Lib-Api-CellAreaKm2-H3Lib-H3Index-'></a>
### CellAreaKm2() `method`

##### Summary

exact area for a specific cell (hexagon or pentagon) in kilometers^2

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-CellAreaM2-H3Lib-H3Index-'></a>
### CellAreaM2() `method`

##### Summary

exact area for a specific cell (hexagon or pentagon) in meters^2

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-CellAreaRads2-H3Lib-H3Index-'></a>
### CellAreaRads2() `method`

##### Summary

exact area for a specific cell (hexagon or pentagon) in radians^2

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-Compact-System-Collections-Generic-List{H3Lib-H3Index},System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### Compact() `method`

##### Summary

compacts the given set of hexagons as best as possible

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-DegreesToRadians-System-Double-'></a>
### DegreesToRadians(degrees) `method`

##### Summary

converts degrees to radians

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| degrees | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') |  |

<a name='M-H3Lib-Api-EdgeLengthKm-System-Int32-'></a>
### EdgeLengthKm() `method`

##### Summary

average hexagon edge length in kilometers (excludes pentagons)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-EdgeLengthM-System-Int32-'></a>
### EdgeLengthM() `method`

##### Summary

average hexagon edge length in meters (excludes pentagons)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-ExactEdgeLengthKm-H3Lib-H3Index-'></a>
### ExactEdgeLengthKm() `method`

##### Summary

exact length for a specific unidirectional edge in kilometers*/

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-ExactEdgeLengthM-H3Lib-H3Index-'></a>
### ExactEdgeLengthM() `method`

##### Summary

exact length for a specific unidirectional edge in meters*/

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-ExactEdgeLengthRads-H3Lib-H3Index-'></a>
### ExactEdgeLengthRads() `method`

##### Summary

exact length for a specific unidirectional edge in radians*/

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-ExperimentalH3ToLocalIj-H3Lib-H3Index,H3Lib-H3Index,H3Lib-CoordIj@-'></a>
### ExperimentalH3ToLocalIj() `method`

##### Summary

Returns two dimensional coordinates for the given index

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-ExperimentalLocalIjToH3-H3Lib-H3Index,H3Lib-CoordIj,H3Lib-H3Index@-'></a>
### ExperimentalLocalIjToH3() `method`

##### Summary

Returns index for the given two dimensional coordinates

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GeoToH3-H3Lib-GeoCoord,System-Int32-'></a>
### GeoToH3() `method`

##### Summary

find the H3 index of the resolution res cell containing the lat/lng

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetDestinationH3IndexFromUnidirectionalEdge-H3Lib-H3Index-'></a>
### GetDestinationH3IndexFromUnidirectionalEdge() `method`

##### Summary

Returns the destination hexagon H3Index from the unidirectional edge
H3Index

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetH3IndexesFromUnidirectionalEdge-H3Lib-H3Index,System-ValueTuple{H3Lib-H3Index,H3Lib-H3Index}@-'></a>
### GetH3IndexesFromUnidirectionalEdge() `method`

##### Summary

Returns the origin and destination hexagons from the unidirectional
edge H3Index

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetH3UnidirectionalEdge-H3Lib-H3Index,H3Lib-H3Index-'></a>
### GetH3UnidirectionalEdge() `method`

##### Summary

returns the unidirectional edge H3Index for the specified origin and
destination

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetH3UnidirectionalEdgeBoundary-H3Lib-H3Index,H3Lib-GeoBoundary@-'></a>
### GetH3UnidirectionalEdgeBoundary() `method`

##### Summary

Returns the GeoBoundary containing the coordinates of the edge

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetH3UnidirectionalEdgesFromHexagon-H3Lib-H3Index,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### GetH3UnidirectionalEdgesFromHexagon() `method`

##### Summary

Returns the 6 (or 5 for pentagons) edges associated with the H3Index

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetOriginH3IndexFromUnidirectionalEdge-H3Lib-H3Index-'></a>
### GetOriginH3IndexFromUnidirectionalEdge() `method`

##### Summary

Returns the origin hexagon H3Index from the unidirectional edge

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetPentagonIndexes-System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### GetPentagonIndexes() `method`

##### Summary

generates all pentagons at the specified resolution

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-GetRes0Indexes-System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### GetRes0Indexes() `method`

##### Summary

provides all base cells in H3Index format*/

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3Distance-H3Lib-H3Index,H3Lib-H3Index-'></a>
### H3Distance() `method`

##### Summary

Returns grid distance between two indexes

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3GetBaseCell-H3Lib-H3Index-'></a>
### H3GetBaseCell() `method`

##### Summary

returns the base cell "number" (0 to 121) of the provided H3 cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3GetFaces-H3Lib-H3Index,System-Collections-Generic-List{System-Int32}@-'></a>
### H3GetFaces() `method`

##### Summary

Find all icosahedron faces intersected by a given H3 index

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3GetResolution-H3Lib-H3Index-'></a>
### H3GetResolution() `method`

##### Summary

returns the resolution of the provided H3 index
Works on both cells and unidirectional edges.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3IndexesAreNeighbors-H3Lib-H3Index,H3Lib-H3Index-'></a>
### H3IndexesAreNeighbors() `method`

##### Summary

returns whether or not the provided hexagons border

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3IsPentagon-H3Lib-H3Index-'></a>
### H3IsPentagon() `method`

##### Summary

determines if an H3 cell is a pentagon

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3IsResClassIii-H3Lib-H3Index-'></a>
### H3IsResClassIii() `method`

##### Summary

determines if a hexagon is Class III (or Class II)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3IsValid-H3Lib-H3Index-'></a>
### H3IsValid() `method`

##### Summary

confirms if an H3Index is a valid cell (hexagon or pentagon)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3Line-H3Lib-H3Index,H3Lib-H3Index,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### H3Line() `method`

##### Summary

Line of h3 indexes connecting two indexes

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3LineSize-H3Lib-H3Index,H3Lib-H3Index-'></a>
### H3LineSize() `method`

##### Summary

Number of indexes in a line connecting two indexes

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3SetToLinkedGeo-System-Collections-Generic-List{H3Lib-H3Index},H3Lib-LinkedGeoPolygon@-'></a>
### H3SetToLinkedGeo(h3Set,outPolygon) `method`

##### Summary

Create a LinkedGeoPolygon from a set of contiguous hexagons

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3Set | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') |  |
| outPolygon | [H3Lib.LinkedGeoPolygon@](#T-H3Lib-LinkedGeoPolygon@ 'H3Lib.LinkedGeoPolygon@') |  |

<a name='M-H3Lib-Api-H3ToCenterChild-H3Lib-H3Index,System-Int32-'></a>
### H3ToCenterChild() `method`

##### Summary

returns the center child of the given hexagon at the specified

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3ToChildren-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### H3ToChildren() `method`

##### Summary

provides the children (or grandchildren, etc) of the given hexagon

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3ToGeo-H3Lib-H3Index,H3Lib-GeoCoord@-'></a>
### H3ToGeo() `method`

##### Summary

find the lat/lon center point g of the cell h3

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3ToGeoBoundary-H3Lib-H3Index,H3Lib-GeoBoundary@-'></a>
### H3ToGeoBoundary() `method`

##### Summary

give the cell boundary in lat/lon coordinates for the cell h3

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3ToParent-H3Lib-H3Index,System-Int32-'></a>
### H3ToParent() `method`

##### Summary

returns the parent (or grandparent, etc) hexagon of the given hexagon

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3ToString-H3Lib-H3Index,System-String@-'></a>
### H3ToString() `method`

##### Summary

converts an H3Index to a canonical string

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-H3UnidirectionalEdgeIsValid-H3Lib-H3Index-'></a>
### H3UnidirectionalEdgeIsValid() `method`

##### Summary

returns whether the H3Index is a valid unidirectional edge

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-HexAreaKm2-System-Int32-'></a>
### HexAreaKm2() `method`

##### Summary

average hexagon area in square kilometers (excludes pentagons)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-HexAreaM2-System-Int32-'></a>
### HexAreaM2() `method`

##### Summary

average hexagon area in square meters (excludes pentagons)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-HexRange-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### HexRange() `method`

##### Summary

hexagons neighbors in all directions, assuming no pentagons

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-HexRangeDistances-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@,System-Collections-Generic-List{System-Int32}@-'></a>
### HexRangeDistances() `method`

##### Summary

hexagons neighbors in all directions, assuming no pentagons,
reporting distance from origin

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-HexRanges-System-Collections-Generic-List{H3Lib-H3Index},System-Int32,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### HexRanges() `method`

##### Summary

collection of hex rings sorted by ring for all given hexagons

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-HexRing-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### HexRing(origin,k,outCells) `method`

##### Summary

hollow hexagon ring at some origin

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') |  |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| outCells | [System.Collections.Generic.List{H3Lib.H3Index}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}@') |  |

<a name='M-H3Lib-Api-KRing-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### KRing() `method`

##### Summary

hexagon neighbors in all directions

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-KRingDistances-H3Lib-H3Index,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@,System-Collections-Generic-List{System-Int32}@-'></a>
### KRingDistances() `method`

##### Summary

hexagon neighbors in all directions, reporting distance from origin

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-MaxFaceCount-H3Lib-H3Index-'></a>
### MaxFaceCount() `method`

##### Summary

Max number of icosahedron faces intersected by an index

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-MaxH3ToChildrenSize-H3Lib-H3Index,System-Int32-'></a>
### MaxH3ToChildrenSize() `method`

##### Summary

determines the maximum number of children (or grandchildren, etc)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-MaxKringSize-System-Int32-'></a>
### MaxKringSize() `method`

##### Summary

maximum number of hexagons in k-ring

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-MaxPolyFillSize-H3Lib-GeoPolygon,System-Int32-'></a>
### MaxPolyFillSize(polygon,r) `method`

##### Summary

maximum number of hexagons in the geofence

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| polygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') |  |
| r | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-H3Lib-Api-MaxUncompactSize-H3Lib-H3Index,System-Int32-'></a>
### MaxUncompactSize() `method`

##### Summary

determines the maximum number of hexagons that could be uncompacted
from the compacted set

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-NumHexagons-System-Int32-'></a>
### NumHexagons() `method`

##### Summary

number of cells (hexagons and pentagons) for a given resolution

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-PentagonIndexCount'></a>
### PentagonIndexCount() `method`

##### Summary

returns the number of pentagons per resolution

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-PointDistKm-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### PointDistKm(a,b) `method`

##### Summary

"great circle distance" between pairs of GeoCoord points in kilometers

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |
| b | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |

<a name='M-H3Lib-Api-PointDistM-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### PointDistM() `method`

##### Summary

"great circle distance" between pairs of GeoCoord points in meters*/

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-PointDistRads-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### PointDistRads(a,b) `method`

##### Summary

"great circle distance" between pairs of GeoCoord points in radians*/

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |
| b | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |

<a name='M-H3Lib-Api-PolyFill-H3Lib-GeoPolygon,System-Int32,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### PolyFill(polygon,r,outCells) `method`

##### Summary

hexagons within the given geofence

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| polygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') |  |
| r | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| outCells | [System.Collections.Generic.List{H3Lib.H3Index}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}@') |  |

<a name='M-H3Lib-Api-RadiansToDegrees-System-Double-'></a>
### RadiansToDegrees() `method`

##### Summary

converts radians to degrees

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-Res0IndexCount'></a>
### Res0IndexCount() `method`

##### Summary

returns the number of resolution 0 cells (hexagons and pentagons)

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-SetGeoDegs-System-Double,System-Double-'></a>
### SetGeoDegs() `method`

##### Summary

Winging this one, returns a GeoCoord with degree values instead of radians

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-StringToH3-System-String-'></a>
### StringToH3() `method`

##### Summary

converts the canonical string format to H3Index format

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Api-Uncompact-System-Collections-Generic-List{H3Lib-H3Index},System-Collections-Generic-List{H3Lib-H3Index}@,System-Int32-'></a>
### Uncompact() `method`

##### Summary

uncompacts the compacted hexagon set

##### Parameters

This method has no parameters.

<a name='T-H3Lib-BBox'></a>
## BBox `type`

##### Namespace

H3Lib

##### Summary

Geographic bounding box with coordinates defined in radians

<a name='M-H3Lib-BBox-#ctor-System-Double,System-Double,System-Double,System-Double-'></a>
### #ctor() `constructor`

##### Summary

constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-BBox-East'></a>
### East `constants`

##### Summary

East limit

<a name='F-H3Lib-BBox-North'></a>
### North `constants`

##### Summary

North limit

<a name='F-H3Lib-BBox-South'></a>
### South `constants`

##### Summary

South limit

<a name='F-H3Lib-BBox-West'></a>
### West `constants`

##### Summary

West limit

<a name='P-H3Lib-BBox-IsTransmeridian'></a>
### IsTransmeridian `property`

##### Summary

Whether the given bounding box crosses the antimeridian

<a name='M-H3Lib-BBox-Equals-H3Lib-BBox-'></a>
### Equals() `method`

##### Summary

Test for equality within measure of error against other BBox

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BBox-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Test for object that can be unboxed to BBox

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BBox-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-BBox-op_Equality-H3Lib-BBox,H3Lib-BBox-'></a>
### op_Equality() `method`

##### Summary

Test for equality

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BBox-op_Inequality-H3Lib-BBox,H3Lib-BBox-'></a>
### op_Inequality() `method`

##### Summary

Test for inequality

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-BBoxExtensions'></a>
## BBoxExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Extension methods for BBoxes

<a name='M-H3Lib-Extensions-BBoxExtensions-Center-H3Lib-BBox-'></a>
### Center(box) `method`

##### Summary

Gets the center of a bounding box

##### Returns

output center coordinate

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | input bounding box |

<a name='M-H3Lib-Extensions-BBoxExtensions-Contains-H3Lib-BBox,H3Lib-GeoCoord-'></a>
### Contains(box,point) `method`

##### Summary

Whether the bounding box contains a given point

##### Returns

Whether the point is contained

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | Bounding box |
| point | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | Point to test |

<a name='M-H3Lib-Extensions-BBoxExtensions-HexEstimate-H3Lib-BBox,System-Int32-'></a>
### HexEstimate(box,res) `method`

##### Summary

returns an estimated number of hexagons that fit
within the cartesian-projected bounding box

##### Returns

estimated number of hexagons to fill the bounding box

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | bounding box to estimate the hexagon fill level |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | resolution of the H3 hexagons to fill the bounding box |

<a name='M-H3Lib-Extensions-BBoxExtensions-ReplaceEW-H3Lib-BBox,System-Double,System-Double-'></a>
### ReplaceEW(box,e,w) `method`

##### Summary

Returns a new BBox with replaced East/West values.

 Relevant for Transmeridian issues.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | box to replace |
| e | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | East value |
| w | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | West value |

<a name='M-H3Lib-Extensions-BBoxExtensions-ReplaceEast-H3Lib-BBox,System-Double-'></a>
### ReplaceEast(box,e) `method`

##### Summary

Returns a new BBox with the new East value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | box to replace |
| e | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | East Value |

<a name='M-H3Lib-Extensions-BBoxExtensions-ReplaceNorth-H3Lib-BBox,System-Double-'></a>
### ReplaceNorth(box,n) `method`

##### Summary

Returns a new BBox with the new North value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | box to replace |
| n | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | North Value |

<a name='M-H3Lib-Extensions-BBoxExtensions-ReplaceSouth-H3Lib-BBox,System-Double-'></a>
### ReplaceSouth(box,s) `method`

##### Summary

Returns a new BBox with the new South value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | box to replace |
| s | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | South Value |

<a name='M-H3Lib-Extensions-BBoxExtensions-ReplaceWest-H3Lib-BBox,System-Double-'></a>
### ReplaceWest(box,w) `method`

##### Summary

Returns a new BBox with the new West value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') | box to replace |
| w | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | West Value |

<a name='T-H3Lib-BaseCellData'></a>
## BaseCellData `type`

##### Namespace

H3Lib

##### Summary

Information on a single base cell

<a name='M-H3Lib-BaseCellData-#ctor-System-Int32,System-Int32,System-Int32,System-Int32,System-Int32,System-Int32,System-Int32-'></a>
### #ctor(face,faceI,faceJ,faceK,isPentagon,offset1,offset2) `constructor`

##### Summary

Extended constructor

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| face | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Face of BaseCellData |
| faceI | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | I coordinate |
| faceJ | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | J Coordinate |
| faceK | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | K Coordinate |
| isPentagon | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Is cell pentagon? |
| offset1 | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | offset 1 |
| offset2 | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | offset 2 |

<a name='F-H3Lib-BaseCellData-ClockwiseOffsetPentagon'></a>
### ClockwiseOffsetPentagon `constants`

##### Summary

If it's a pentagon, what are its two clockwise offset faces?

<a name='F-H3Lib-BaseCellData-HomeFijk'></a>
### HomeFijk `constants`

##### Summary

"Home" face and normalized ijk coordinates on that face

<a name='F-H3Lib-BaseCellData-IsPentagon'></a>
### IsPentagon `constants`

##### Summary

Is this base cell a pentagon?

<a name='M-H3Lib-BaseCellData-Equals-H3Lib-BaseCellData-'></a>
### Equals(other) `method`

##### Summary

Test for equality

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| other | [H3Lib.BaseCellData](#T-H3Lib-BaseCellData 'H3Lib.BaseCellData') | BaseCellData to test against |

<a name='M-H3Lib-BaseCellData-Equals-System-Object-'></a>
### Equals(obj) `method`

##### Summary

Test for equality against object that can be unboxed

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| obj | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | Object to unbox if BaseCellData |

<a name='M-H3Lib-BaseCellData-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

HashCode for identity

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BaseCellData-op_Equality-H3Lib-BaseCellData,H3Lib-BaseCellData-'></a>
### op_Equality(left,right) `method`

##### Summary

Test for equality

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| left | [H3Lib.BaseCellData](#T-H3Lib-BaseCellData 'H3Lib.BaseCellData') | lhs item |
| right | [H3Lib.BaseCellData](#T-H3Lib-BaseCellData 'H3Lib.BaseCellData') | rhs item |

<a name='M-H3Lib-BaseCellData-op_Inequality-H3Lib-BaseCellData,H3Lib-BaseCellData-'></a>
### op_Inequality(left,right) `method`

##### Summary

Test for inequality

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| left | [H3Lib.BaseCellData](#T-H3Lib-BaseCellData 'H3Lib.BaseCellData') | lhs item |
| right | [H3Lib.BaseCellData](#T-H3Lib-BaseCellData 'H3Lib.BaseCellData') | rhd item |

<a name='T-H3Lib-BaseCellRotation'></a>
## BaseCellRotation `type`

##### Namespace

H3Lib

##### Summary

base cell at a given ijk and required rotations into its system

<a name='M-H3Lib-BaseCellRotation-#ctor-System-Int32,System-Int32-'></a>
### #ctor() `constructor`

##### Summary

constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-BaseCellRotation-BaseCell'></a>
### BaseCell `constants`

##### Summary

base cell number

<a name='F-H3Lib-BaseCellRotation-CounterClockwiseRotate60'></a>
### CounterClockwiseRotate60 `constants`

##### Summary

number of ccw 60 degree rotations relative to current face

<a name='M-H3Lib-BaseCellRotation-Equals-H3Lib-BaseCellRotation-'></a>
### Equals() `method`

##### Summary

Test for equality against BaseCellRotation

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BaseCellRotation-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Test for equality against object that can be unboxed to BaseCellRotation

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BaseCellRotation-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BaseCellRotation-op_Equality-H3Lib-BaseCellRotation,H3Lib-BaseCellRotation-'></a>
### op_Equality() `method`

##### Summary

Test for equality

##### Parameters

This method has no parameters.

<a name='M-H3Lib-BaseCellRotation-op_Inequality-H3Lib-BaseCellRotation,H3Lib-BaseCellRotation-'></a>
### op_Inequality() `method`

##### Summary

Test for inequality

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Constants-BaseCells'></a>
## BaseCells `type`

##### Namespace

H3Lib.Constants

<a name='F-H3Lib-Constants-BaseCells-BaseCellData'></a>
### BaseCellData `constants`

##### Summary

Resolution 0 base cell data table.

 For each base cell, gives the "home" face and ijk+ coordinates on that face,
 whether or not the base cell is a pentagon. Additionally, if the base cell
 is a pentagon, the two cw offset rotation adjacent faces are given (-1
 indicates that no cw offset rotation faces exist for this base cell).

<a name='F-H3Lib-Constants-BaseCells-BaseCellNeighbor60CounterClockwiseRotation'></a>
### BaseCellNeighbor60CounterClockwiseRotation `constants`

##### Summary

Neighboring base cell rotations in each IJK direction.

 For each base cell, for each direction, the number of 60 degree
 CCW rotations to the coordinate system of the neighbor is given.
 -1 indicates there is no neighbor in that direction.

<a name='F-H3Lib-Constants-BaseCells-BaseCellNeighbors'></a>
### BaseCellNeighbors `constants`

##### Summary

Neighboring base cell ID in each IJK direction.

 For each base cell, for each direction, the neighboring base
 cell ID is given. 127 indicates there is no neighbor in that direction.

<a name='F-H3Lib-Constants-BaseCells-FaceIjkBaseCells'></a>
### FaceIjkBaseCells `constants`

##### Summary

Resolution 0 base cell lookup table for each face.

 Given the face number and a resolution 0 ijk+ coordinate in that face's
 face-centered ijk coordinate system, gives the base cell located at that
 coordinate and the number of 60 ccw rotations to rotate into that base
 cell's orientation.

 Valid lookup coordinates are from (0, 0, 0) to (2, 2, 2).

 This table can be accessed using the functions BaseCells._faceIjkToBaseCell
 and BaseCells.ToBaseCellCounterClockwiseRotate60

<a name='F-H3Lib-Constants-BaseCells-InvalidRotations'></a>
### InvalidRotations `constants`

##### Summary

Invalid number of rotations

<a name='F-H3Lib-Constants-BaseCells-MaxFaceCoord'></a>
### MaxFaceCoord `constants`

##### Summary

Maximum input for any component to face-to-base-cell lookup functions

<a name='T-H3Lib-Extensions-BaseCellsExtensions'></a>
## BaseCellsExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Extension methods for BaseCells

<a name='P-H3Lib-Extensions-BaseCellsExtensions-Res0IndexCount'></a>
### Res0IndexCount `property`

##### Summary

NOTE: Looks like this is not needed.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-GetBaseCellDirection-System-Int32,System-Int32-'></a>
### GetBaseCellDirection() `method`

##### Summary

Return the direction from the origin base cell to the neighbor.

##### Returns

INVALID_DIGIT if the base cells are not neighbors.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-GetNeighbor-System-Int32,H3Lib-Direction-'></a>
### GetNeighbor() `method`

##### Summary

Return the neighboring base cell in the given direction.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-GetRes0Indexes'></a>
### GetRes0Indexes() `method`

##### Summary

getRes0Indexes generates all base cells

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-IsBaseCellPentagon-System-Int32-'></a>
### IsBaseCellPentagon() `method`

##### Summary

Return whether or not the indicated base cell is a pentagon.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-IsBaseCellPolarPentagon-System-Int32-'></a>
### IsBaseCellPolarPentagon() `method`

##### Summary

Return whether the indicated base cell is a pentagon where all
neighbors are oriented towards it.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-IsClockwiseOffset-System-Int32,System-Int32-'></a>
### IsClockwiseOffset() `method`

##### Summary

Return whether or not the tested face is a cw offset face.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-ToCounterClockwiseRotate60-System-Int32,System-Int32-'></a>
### ToCounterClockwiseRotate60() `method`

##### Summary

Given a base cell and the face it appears on, return
the number of 60' ccw rotations for that base cell's
coordinate system.

##### Returns

The number of rotations, or INVALID_ROTATIONS if the base
cell is not found on the given face

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-BaseCellsExtensions-ToFaceIjk-System-Int32-'></a>
### ToFaceIjk() `method`

##### Summary

Find the FaceIJK given a base cell.

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-CollectionExtensions'></a>
## CollectionExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Static methods that work on collections.

Currently List, but will likely be switched to
IEnumerable in future

<a name='M-H3Lib-Extensions-CollectionExtensions-FindDeepestContainer-System-Collections-Generic-List{H3Lib-LinkedGeoPolygon},System-Collections-Generic-List{H3Lib-BBox}-'></a>
### FindDeepestContainer(polygons,boxes) `method`

##### Summary

Given a list of nested containers, find the one most deeply nested.

##### Returns

Deepest container, or null if list is empty

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| polygons | [System.Collections.Generic.List{H3Lib.LinkedGeoPolygon}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.LinkedGeoPolygon}') | Polygon containers to check |
| boxes | [System.Collections.Generic.List{H3Lib.BBox}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.BBox}') | Bounding boxes for polygons, used in point-in-poly check |

<a name='M-H3Lib-Extensions-CollectionExtensions-HexRanges-System-Collections-Generic-List{H3Lib-H3Index},System-Int32-'></a>
### HexRanges(h3Set,k) `method`

##### Summary

hexRanges takes an array of input hex IDs and a max k-ring and returns an
array of hexagon IDs sorted first by the original hex IDs and then by the
k-ring (0 to max), with no guaranteed sorting within each k-ring group.

##### Returns

Tuple
    Item1 - 0 if no pentagon is encountered. Cannot trust output otherwise
    Item2 - List of H3Index cells

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3Set | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') | a list of H3Indexes |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k The number of rings to generate |

<a name='M-H3Lib-Extensions-CollectionExtensions-MaxUncompactSize-System-Collections-Generic-List{H3Lib-H3Index},System-Int32-'></a>
### MaxUncompactSize(compactedSet,res) `method`

##### Summary

maxUncompactSize takes a compacted set of hexagons are provides an
upper-bound estimate of the size of the uncompacted set of hexagons.

##### Returns

The number of hexagons to allocate memory for, or a negative
number if an error occurs.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| compactedSet | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') | Set of hexagons |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The hexagon resolution to decompress to |

<a name='M-H3Lib-Extensions-CollectionExtensions-ToLinkedGeoPolygon-System-Collections-Generic-List{H3Lib-H3Index}-'></a>
### ToLinkedGeoPolygon(h3Set) `method`

##### Summary

Create a LinkedGeoPolygon describing the outline(s) of a set of  hexagons.
Polygon outlines will follow GeoJSON MultiPolygon order: Each polygon will
have one outer loop, which is first in the list, followed by any holes.

It is the responsibility of the caller to call destroyLinkedPolygon on the
populated linked geo structure, or the memory for that structure will
not be freed.

It is expected that all hexagons in the set have the same resolution and
that the set contains no duplicates. Behavior is undefined if duplicates
or multiple resolutions are present, and the algorithm may produce
unexpected or invalid output.

##### Returns

Output polygon

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3Set | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') | Set of Hexagons |

<a name='M-H3Lib-Extensions-CollectionExtensions-ToVertexGraph-System-Collections-Generic-List{H3Lib-H3Index}-'></a>
### ToVertexGraph(h3Set) `method`

##### Summary

Internal: Create a vertex graph from a set of hexagons. It is the
responsibility of the caller to call destroyVertexGraph on the populated
graph, otherwise the memory in the graph nodes will not be freed.

##### Returns

Output graph

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3Set | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') | Set of hexagons |

<a name='M-H3Lib-Extensions-CollectionExtensions-Uncompact-System-Collections-Generic-List{H3Lib-H3Index},System-Int32-'></a>
### Uncompact(compactedSet,res) `method`

##### Summary

uncompact takes a compressed set of hexagons and expands back to the
original set of hexagons.

##### Returns

A status code and the uncompacted hexagons.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| compactedSet | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') | Set of hexagons |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The hexagon resolution to decompress to |

<a name='T-H3Lib-Constants'></a>
## Constants `type`

##### Namespace

H3Lib

##### Summary

Collection of constants used throughout the library.

<a name='F-H3Lib-Constants-H3_VERSION_MAJOR'></a>
### H3_VERSION_MAJOR `constants`

##### Summary

Major version

<a name='F-H3Lib-Constants-H3_VERSION_MINOR'></a>
### H3_VERSION_MINOR `constants`

##### Summary

Minor version

<a name='F-H3Lib-Constants-H3_VERSION_PATCH'></a>
### H3_VERSION_PATCH `constants`

##### Summary

Patch version

<a name='T-H3Lib-CoordIj'></a>
## CoordIj `type`

##### Namespace

H3Lib

##### Summary

IJ Hexagon coordinates.

 Each axis is spaced 120 degrees apart

<a name='M-H3Lib-CoordIj-#ctor-System-Int32,System-Int32-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-CoordIj-#ctor-H3Lib-CoordIj-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-CoordIj-I'></a>
### I `constants`

##### Summary

I Component

<a name='F-H3Lib-CoordIj-J'></a>
### J `constants`

##### Summary

J component

<a name='M-H3Lib-CoordIj-Equals-H3Lib-CoordIj-'></a>
### Equals() `method`

##### Summary

Test for equality

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Test for equality on object that can be unboxed to CoordIJ

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-op_Addition-H3Lib-CoordIj,H3Lib-CoordIj-'></a>
### op_Addition() `method`

##### Summary

Addition operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-op_Equality-H3Lib-CoordIj,H3Lib-CoordIj-'></a>
### op_Equality() `method`

##### Summary

Test for equality

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-op_Inequality-H3Lib-CoordIj,H3Lib-CoordIj-'></a>
### op_Inequality() `method`

##### Summary

Test for inequality

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-op_Multiply-H3Lib-CoordIj,System-Int32-'></a>
### op_Multiply() `method`

##### Summary

Multiply operator for scaling

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIj-op_Subtraction-H3Lib-CoordIj,H3Lib-CoordIj-'></a>
### op_Subtraction() `method`

##### Summary

Subtraction operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-CoordIjExtensions'></a>
## CoordIjExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Extension methods for working with CoordIj type

<a name='M-H3Lib-Extensions-CoordIjExtensions-ReplaceI-H3Lib-CoordIj,System-Int32-'></a>
### ReplaceI() `method`

##### Summary

Replace I value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjExtensions-ReplaceJ-H3Lib-CoordIj,System-Int32-'></a>
### ReplaceJ() `method`

##### Summary

replace J value

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjExtensions-ToH3Experimental-H3Lib-CoordIj,H3Lib-H3Index-'></a>
### ToH3Experimental(ij,origin) `method`

##### Summary

Produces an index for ij coordinates anchored by an origin.

 The coordinate space used by this function may have deleted
 regions or warping due to pentagonal distortion.

 Failure may occur if the index is too far away from the origin
 or if the index is on the other side of a pentagon.

 This function is experimental, and its output is not guaranteed
 to be compatible across different versions of H3.

##### Returns

Tuple:
 Item1 indicates status => 0 = Success, other = failure
 Item2 contains H3Index upon success.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ij | [H3Lib.CoordIj](#T-H3Lib-CoordIj 'H3Lib.CoordIj') | coordinates to index. |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | An anchoring index for the ij coordinate system. |

<a name='M-H3Lib-Extensions-CoordIjExtensions-ToIjk-H3Lib-CoordIj-'></a>
### ToIjk(ij) `method`

##### Summary

Transforms coordinates from the IJ coordinate system to the IJK+ coordinate system

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ij | [H3Lib.CoordIj](#T-H3Lib-CoordIj 'H3Lib.CoordIj') | The input IJ coordinates |

##### Remarks

coordijk.c
void ijToIjk

<a name='T-H3Lib-Constants-CoordIjk'></a>
## CoordIjk `type`

##### Namespace

H3Lib.Constants

<a name='T-H3Lib-CoordIjk'></a>
## CoordIjk `type`

##### Namespace

H3Lib

##### Summary

Header file for CoordIJK functions including conversion from lat/lon

##### Remarks

References two Vec2d cartesian coordinate systems:

 1. gnomonic: face-centered polyhedral gnomonic projection space with
    traditional scaling and x-axes aligned with the face Class II
    i-axes

 2. hex2d: local face-centered coordinate system scaled a specific H3 grid
    resolution unit length and with x-axes aligned with the local i-axes

<a name='M-H3Lib-CoordIjk-#ctor-System-Int32,System-Int32,System-Int32-'></a>
### #ctor() `constructor`

##### Summary

IJK hexagon coordinates

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-CoordIjk-#ctor-H3Lib-CoordIjk-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-Constants-CoordIjk-UnitVecs'></a>
### UnitVecs `constants`

##### Summary

CoordIJK unit vectors corresponding to the 7 H3 digits.

<a name='F-H3Lib-CoordIjk-I'></a>
### I `constants`

##### Summary

I Coordinate

<a name='F-H3Lib-CoordIjk-J'></a>
### J `constants`

##### Summary

J Coordinate

<a name='F-H3Lib-CoordIjk-K'></a>
### K `constants`

##### Summary

K Coordinate

<a name='M-H3Lib-CoordIjk-CubeRound-System-Double,System-Double,System-Double-'></a>
### CubeRound(i,j,k) `method`

##### Summary

Given cube coords as doubles, round to valid integer coordinates. Algorithm
from https://www.redblobgames.com/grids/hexagons/#rounding

##### Returns

IJK coord struct

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| i | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | Floating-point I coord |
| j | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | Floating-point J coord |
| k | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | Floating-point K coord |

<a name='M-H3Lib-CoordIjk-Equals-H3Lib-CoordIjk-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality for unboxed object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-ToString'></a>
### ToString() `method`

##### Summary

Debug information

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-op_Addition-H3Lib-CoordIjk,H3Lib-CoordIjk-'></a>
### op_Addition() `method`

##### Summary

Addition operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-op_Equality-H3Lib-CoordIjk,H3Lib-CoordIjk-'></a>
### op_Equality() `method`

##### Summary

Equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-op_Inequality-H3Lib-CoordIjk,H3Lib-CoordIjk-'></a>
### op_Inequality() `method`

##### Summary

Inequality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-op_Multiply-H3Lib-CoordIjk,System-Int32-'></a>
### op_Multiply() `method`

##### Summary

Multiply operator for scaling

##### Parameters

This method has no parameters.

<a name='M-H3Lib-CoordIjk-op_Subtraction-H3Lib-CoordIjk,H3Lib-CoordIjk-'></a>
### op_Subtraction() `method`

##### Summary

Subtraction operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-CoordIjkExtensions'></a>
## CoordIjkExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Extension methods for CoordIjk type

<a name='M-H3Lib-Extensions-CoordIjkExtensions-DistanceTo-H3Lib-CoordIjk,H3Lib-CoordIjk-'></a>
### DistanceTo(start,end) `method`

##### Summary

Finds the distance between the two coordinates. Returns result.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| start | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The first set of ijk coordinates. |
| end | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The second set of ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-DownAp3-H3Lib-CoordIjk-'></a>
### DownAp3(ijk) `method`

##### Summary

Find the normalized ijk coordinates of the hex centered on the indicated
hex at the next finer aperture 3 counter-clockwise resolution. Works in
place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-DownAp3R-H3Lib-CoordIjk-'></a>
### DownAp3R(ijk) `method`

##### Summary

Find the normalized ijk coordinates of the hex centered on the indicated
hex at the next finer aperture 3 clockwise resolution. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-DownAp7-H3Lib-CoordIjk-'></a>
### DownAp7(ijk) `method`

##### Summary

Find the normalized ijk coordinates of the hex centered on the indicated
hex at the next finer aperture 7 counter-clockwise resolution. Works in
place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-DownAp7R-H3Lib-CoordIjk-'></a>
### DownAp7R(ijk) `method`

##### Summary

Find the normalized ijk coordinates of the hex centered on the indicated
hex at the next finer aperture 7 clockwise resolution. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-FromCube-H3Lib-CoordIjk-'></a>
### FromCube(ijk) `method`

##### Summary

Convert cube coordinates to IJK coordinates, in place

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | Coordinate to convert |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-IsZero-H3Lib-CoordIjk-'></a>
### IsZero() `method`

##### Summary

Tests if all coordinates are zero

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-LocalIjkToH3-H3Lib-CoordIjk,H3Lib-H3Index-'></a>
### LocalIjkToH3(origin,ijk) `method`

##### Summary

Produces an index for ijk+ coordinates anchored by an origin.

 The coordinate space used by this function may have deleted
 regions or warping due to pentagonal distortion.

 Failure may occur if the coordinates are too far away from the origin
 or if the index is on the other side of a pentagon.

##### Returns

0 on success, or another value on failure

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | An anchoring index for the ijk+ coordinate system. |
| ijk | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | IJK+ Coordinates to find the index of |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-Neighbor-H3Lib-CoordIjk,H3Lib-Direction-'></a>
### Neighbor(ijk,digit) `method`

##### Summary

Find the normalized ijk coordinates of the hex in the specified digit
direction from the specified ijk coordinates. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates. |
| digit | [H3Lib.Direction](#T-H3Lib-Direction 'H3Lib.Direction') | The digit direction from the original ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-Normalized-H3Lib-CoordIjk-'></a>
### Normalized(coord) `method`

##### Summary

Normalizes ijk coordinates by setting the components to the smallest possible
values. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| coord | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates to normalize. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-Rotate60Clockwise-H3Lib-CoordIjk-'></a>
### Rotate60Clockwise(ijk) `method`

##### Summary

Rotates ijk coordinates 60 degrees clockwise. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-Rotate60CounterClockwise-H3Lib-CoordIjk-'></a>
### Rotate60CounterClockwise(ijk) `method`

##### Summary

Rotates ijk coordinates 60 degrees counter-clockwise. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-SetI-H3Lib-CoordIjk,System-Int32-'></a>
### SetI() `method`

##### Summary

Change I coordinate value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-SetIJ-H3Lib-CoordIjk,System-Int32,System-Int32-'></a>
### SetIJ() `method`

##### Summary

Change IJ coordinates value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-SetIK-H3Lib-CoordIjk,System-Int32,System-Int32-'></a>
### SetIK() `method`

##### Summary

Change JK coordinates value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-SetJ-H3Lib-CoordIjk,System-Int32-'></a>
### SetJ() `method`

##### Summary

Change J coordinate value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-SetJK-H3Lib-CoordIjk,System-Int32,System-Int32-'></a>
### SetJK() `method`

##### Summary

Change JK coordinates value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-SetK-H3Lib-CoordIjk,System-Int32-'></a>
### SetK() `method`

##### Summary

Change K coordinate value

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-Sum-H3Lib-CoordIjk-'></a>
### Sum() `method`

##### Summary

Returns sum of all coordinates

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-CoordIjkExtensions-ToCube-H3Lib-CoordIjk-'></a>
### ToCube(ijk) `method`

##### Summary

Convert IJK coordinates to cube coordinates, in place

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | Coordinate to convert |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-ToDirection-H3Lib-CoordIjk-'></a>
### ToDirection(ijk) `method`

##### Summary

Determines the H3 digit corresponding to a unit vector in ijk coordinates.

##### Returns

The H3 digit (0-6) corresponding to the ijk unit vector, or
[INVALID_DIGIT](#F-H3Lib-Direction-INVALID_DIGIT 'H3Lib.Direction.INVALID_DIGIT') INVALID_DIGIT on failure

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates; must be a unit vector. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-ToHex2d-H3Lib-CoordIjk-'></a>
### ToHex2d(h) `method`

##### Summary

Find the center point in 2D cartesian coordinates of a hex.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates of the hex. |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-ToIj-H3Lib-CoordIjk-'></a>
### ToIj(ijk) `method`

##### Summary

Transforms coordinates from the IJK+ coordinate system to the IJ coordinate system

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The input IJK+ coordinates |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-UpAp7-H3Lib-CoordIjk-'></a>
### UpAp7(ijk) `method`

##### Summary

Find the normalized ijk coordinates of the indexing parent of a cell in a
counter-clockwise aperture 7 grid. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates |

<a name='M-H3Lib-Extensions-CoordIjkExtensions-UpAp7R-H3Lib-CoordIjk-'></a>
### UpAp7R(ijk) `method`

##### Summary

Find the normalized ijk coordinates of the indexing parent of a cell in a
clockwise aperture 7 grid. Works in place.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ijk | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | The ijk coordinates |

<a name='T-H3Lib-Direction'></a>
## Direction `type`

##### Namespace

H3Lib

##### Summary

H3 digit representing ijk+ axes direction.
Values will be within the lowest 3 bits of an integer.

<a name='F-H3Lib-Direction-CENTER_DIGIT'></a>
### CENTER_DIGIT `constants`

##### Summary

H3 digit in center

<a name='F-H3Lib-Direction-IJ_AXES_DIGIT'></a>
### IJ_AXES_DIGIT `constants`

##### Summary

H3 digit in i==j direction

<a name='F-H3Lib-Direction-IK_AXES_DIGIT'></a>
### IK_AXES_DIGIT `constants`

##### Summary

H3 digit in i==k direction

<a name='F-H3Lib-Direction-INVALID_DIGIT'></a>
### INVALID_DIGIT `constants`

##### Summary

H3 digit in the invalid direction

<a name='F-H3Lib-Direction-I_AXES_DIGIT'></a>
### I_AXES_DIGIT `constants`

##### Summary

H3 digit in i-axes direction

<a name='F-H3Lib-Direction-JK_AXES_DIGIT'></a>
### JK_AXES_DIGIT `constants`

##### Summary

H3 digit in j==k direction

<a name='F-H3Lib-Direction-J_AXES_DIGIT'></a>
### J_AXES_DIGIT `constants`

##### Summary

H3 digit in j-axes direction

<a name='F-H3Lib-Direction-K_AXES_DIGIT'></a>
### K_AXES_DIGIT `constants`

##### Summary

H3 digit in k-axes direction

<a name='F-H3Lib-Direction-NUM_DIGITS'></a>
### NUM_DIGITS `constants`

##### Summary

Valid digits will be less than this value. Same value as [INVALID_DIGIT](#F-H3Lib-Direction-INVALID_DIGIT 'H3Lib.Direction.INVALID_DIGIT')

<a name='T-H3Lib-Extensions-DirectionExtensions'></a>
## DirectionExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations for Direction enum type

<a name='M-H3Lib-Extensions-DirectionExtensions-Rotate60Clockwise-H3Lib-Direction-'></a>
### Rotate60Clockwise(digit) `method`

##### Summary

Rotates indexing digit 60 degrees clockwise. Returns result.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| digit | [H3Lib.Direction](#T-H3Lib-Direction 'H3Lib.Direction') | Indexing digit (between 1 and 6 inclusive) |

<a name='M-H3Lib-Extensions-DirectionExtensions-Rotate60CounterClockwise-H3Lib-Direction-'></a>
### Rotate60CounterClockwise(digit) `method`

##### Summary

Rotates indexing digit 60 degrees counter-clockwise. Returns result.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| digit | [H3Lib.Direction](#T-H3Lib-Direction 'H3Lib.Direction') | Indexing digit (between 1 and 6 inclusive) |

<a name='T-H3Lib-Constants-FaceIjk'></a>
## FaceIjk `type`

##### Namespace

H3Lib.Constants

<a name='T-H3Lib-FaceIjk'></a>
## FaceIjk `type`

##### Namespace

H3Lib

##### Summary

Functions for working with icosahedral face-centered hex IJK
coordinate systems.

<a name='M-H3Lib-FaceIjk-#ctor-System-Int32,H3Lib-CoordIjk-'></a>
### #ctor() `constructor`

##### Summary

constructor

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-FaceIjk-#ctor-H3Lib-FaceIjk-'></a>
### #ctor() `constructor`

##### Summary

constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-Constants-FaceIjk-AdjacentFaceDir'></a>
### AdjacentFaceDir `constants`

##### Summary

direction from the origin face to the destination face, relative to
the origin face's coordinate system, or -1 if not adjacent.

<a name='F-H3Lib-Constants-FaceIjk-FaceAxesAzRadsCii'></a>
### FaceAxesAzRadsCii `constants`

##### Summary

icosahedron face ijk axes as azimuth in radians from face center to
vertex 0/1/2 respectively

<a name='F-H3Lib-Constants-FaceIjk-FaceCenterGeo'></a>
### FaceCenterGeo `constants`

##### Summary

icosahedron face centers in lat/lon radians

<a name='F-H3Lib-Constants-FaceIjk-FaceCenterPoint'></a>
### FaceCenterPoint `constants`

##### Summary

icosahedron face centers in x/y/z on the unit sphere

<a name='F-H3Lib-Constants-FaceIjk-FaceNeighbors'></a>
### FaceNeighbors `constants`

##### Summary

Definition of which faces neighbor each other.

<a name='F-H3Lib-Constants-FaceIjk-IJ'></a>
### IJ `constants`

##### Summary

IJ quadrant faceNeighbors table direction

<a name='F-H3Lib-Constants-FaceIjk-InvalidFace'></a>
### InvalidFace `constants`

##### Summary

Invalid face index

<a name='F-H3Lib-Constants-FaceIjk-JK'></a>
### JK `constants`

##### Summary

JK quadrant faceNeighbors table direction

<a name='F-H3Lib-Constants-FaceIjk-KI'></a>
### KI `constants`

##### Summary

KI quadrant faceNeighbors table direction

<a name='F-H3Lib-Constants-FaceIjk-MSqrt7'></a>
### MSqrt7 `constants`

##### Summary

Square root of 7

<a name='F-H3Lib-Constants-FaceIjk-MaxDimByCiiRes'></a>
### MaxDimByCiiRes `constants`

##### Summary

overage distance table

<a name='F-H3Lib-Constants-FaceIjk-UnitScaleByCiiRes'></a>
### UnitScaleByCiiRes `constants`

##### Summary

unit scale distance table

<a name='F-H3Lib-FaceIjk-Coord'></a>
### Coord `constants`

##### Summary

ijk coordinates on that face

<a name='F-H3Lib-FaceIjk-Face'></a>
### Face `constants`

##### Summary

face number

<a name='M-H3Lib-FaceIjk-Equals-H3Lib-FaceIjk-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceIjk-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality test on unboxed object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceIjk-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceIjk-ToString'></a>
### ToString() `method`

##### Summary

Debug data in string

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceIjk-op_Equality-H3Lib-FaceIjk,H3Lib-FaceIjk-'></a>
### op_Equality() `method`

##### Summary

Equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceIjk-op_Inequality-H3Lib-FaceIjk,H3Lib-FaceIjk-'></a>
### op_Inequality() `method`

##### Summary

Inequality operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-FaceIjkExtensions'></a>
## FaceIjkExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations for FaceIjk type

<a name='M-H3Lib-Extensions-FaceIjkExtensions-AdjustOverageClassIi-H3Lib-FaceIjk,System-Int32,System-Int32,System-Int32-'></a>
### AdjustOverageClassIi(fijk,res,pentLeading4,substrate) `method`

##### Summary

Adjusts a FaceIJK address in place so that the resulting cell address is
relative to the correct icosahedral face.

##### Returns

Tuple
Item1: [Overage](#T-H3Lib-Overage 'H3Lib.Overage')
Item2: Adjusted [FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk')

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the cell. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell. |
| pentLeading4 | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Whether or not the cell is a pentagon with a leading figit 4 |
| substrate | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Whether or not the cell is in a substrate grid. |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-AdjustPentOverage-H3Lib-FaceIjk,System-Int32-'></a>
### AdjustPentOverage(fijk,res) `method`

##### Summary

Adjusts a FaceIJK address for a pentagon vertex in a substrate grid in
place so that the resulting cell address is relative to the correct
icosahedral face.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the cell. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell. |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-PentToGeoBoundary-H3Lib-FaceIjk,System-Int32,System-Int32,System-Int32-'></a>
### PentToGeoBoundary(h,res,start,length) `method`

##### Summary

Generates the cell boundary in spherical coordinates for a pentagonal cell
given by a FaceIJK address at a specified resolution.

##### Returns

The spherical coordinates of the cell boundary.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the pentagonal cell. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell. |
| start | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The first topological vertex to return. |
| length | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number of topological vertexes to return. |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-PentToVerts-H3Lib-FaceIjk,System-Int32,System-Collections-Generic-IList{H3Lib-FaceIjk}-'></a>
### PentToVerts(fijk,res,fijkVerts) `method`

##### Summary

Get the vertices of a pentagon cell as substrate FaceIJK addresses

##### Returns

Tuple
Item1 Possibly modified fijk
Item2 Possibly modified res
Item3 Array for vertices

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the cell. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell. This may be adjusted if
necessary for the substrate grid resolution. |
| fijkVerts | [System.Collections.Generic.IList{H3Lib.FaceIjk}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IList 'System.Collections.Generic.IList{H3Lib.FaceIjk}') | array for the vertices |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ReplaceCoord-H3Lib-FaceIjk,H3Lib-CoordIjk-'></a>
### ReplaceCoord(fijk,coord) `method`

##### Summary

Quick replacement of Coord value

##### Returns

A new instance with the correct values

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | FaceIjk to replace Coord value of |
| coord | [H3Lib.CoordIjk](#T-H3Lib-CoordIjk 'H3Lib.CoordIjk') | New CoordIjk to slot in |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ReplaceFace-H3Lib-FaceIjk,System-Int32-'></a>
### ReplaceFace(fijk,face) `method`

##### Summary

Quick replacement of Face value

##### Returns

A new instance with the correct values

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | FaceIjk to replace Face value of |
| face | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | new Face value to slot in |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ToBaseCell-H3Lib-FaceIjk-'></a>
### ToBaseCell() `method`

##### Summary

Find base cell given FaceIJK.

 Given the face number and a resolution 0 ijk+ coordinate in that face's
 face-centered ijk coordinate system, return the base cell located at that
 coordinate.

 Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ToBaseCellCounterClockwiseRotate60-H3Lib-FaceIjk-'></a>
### ToBaseCellCounterClockwiseRotate60() `method`

##### Summary

Find base cell given FaceIJK.

 Given the face number and a resolution 0 ijk+ coordinate in that face's
 face-centered ijk coordinate system, return the number of 60' ccw rotations
 to rotate into the coordinate system of the base cell at that coordinates.

 Valid ijk+ lookup coordinates are from (0, 0, 0) to (2, 2, 2).

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ToGeoBoundary-H3Lib-FaceIjk,System-Int32,System-Int32,System-Int32-'></a>
### ToGeoBoundary(h,res,start,length) `method`

##### Summary

Generates the cell boundary in spherical coordinates for a cell given by a
FaceIJK address at a specified resolution.

##### Returns

The spherical coordinates of the cell boundary

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the cell |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell |
| start | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The first topological vertex to return |
| length | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number of topological vertexes to return |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ToGeoCoord-H3Lib-FaceIjk,System-Int32-'></a>
### ToGeoCoord(h,res) `method`

##### Summary

Determines the center point in spherical coordinates of a cell given by
a FaceIJK address at a specified resolution.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the cell. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell. |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ToH3-H3Lib-FaceIjk,System-Int32-'></a>
### ToH3(fijk,res) `method`

##### Summary

Convert an FaceIJK address to the corresponding H3Index.

##### Returns

The encoded H3Index (or H3_NULL on failure).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The cell resolution. |

<a name='M-H3Lib-Extensions-FaceIjkExtensions-ToVerts-H3Lib-FaceIjk,System-Int32,System-Collections-Generic-IList{H3Lib-FaceIjk}-'></a>
### ToVerts(fijk,res,fijkVerts) `method`

##### Summary

Get the vertices of a cell as substrate FaceIJK addresses

##### Returns

Tuple
Item1 Possibly modified fijk
Item2 Possibly modified res
Item3 Array for vertices

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIJK address of the cell. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell. This may be adjusted if
necessary for the substrate grid resolution. |
| fijkVerts | [System.Collections.Generic.IList{H3Lib.FaceIjk}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IList 'System.Collections.Generic.IList{H3Lib.FaceIjk}') | array for the vertices |

<a name='T-H3Lib-FaceOrientIjk'></a>
## FaceOrientIjk `type`

##### Namespace

H3Lib

##### Summary

Information to transform into an adjacent face IJK system

<a name='M-H3Lib-FaceOrientIjk-#ctor-System-Int32,System-Int32,System-Int32,System-Int32,System-Int32-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-FaceOrientIjk-#ctor-System-Int32,H3Lib-CoordIjk,System-Int32-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-FaceOrientIjk-Ccw60Rotations'></a>
### Ccw60Rotations `constants`

##### Summary

number of 60 degree ccw rotations relative to primary

<a name='F-H3Lib-FaceOrientIjk-Face'></a>
### Face `constants`

##### Summary

face number

<a name='F-H3Lib-FaceOrientIjk-Translate'></a>
### Translate `constants`

##### Summary

res 0 translation relative to primary face

<a name='M-H3Lib-FaceOrientIjk-Equals-H3Lib-FaceOrientIjk-'></a>
### Equals(other) `method`

##### Summary

Equality test

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| other | [H3Lib.FaceOrientIjk](#T-H3Lib-FaceOrientIjk 'H3Lib.FaceOrientIjk') |  |

<a name='M-H3Lib-FaceOrientIjk-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality test against unboxed object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceOrientIjk-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceOrientIjk-op_Equality-H3Lib-FaceOrientIjk,H3Lib-FaceOrientIjk-'></a>
### op_Equality() `method`

##### Summary

Equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-FaceOrientIjk-op_Inequality-H3Lib-FaceOrientIjk,H3Lib-FaceOrientIjk-'></a>
### op_Inequality() `method`

##### Summary

Inequality operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-GeoBoundary'></a>
## GeoBoundary `type`

##### Namespace

H3Lib

##### Summary

cell boundary in latitude/longitude

<a name='M-H3Lib-GeoBoundary-#ctor'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-GeoBoundary-NumVerts'></a>
### NumVerts `constants`

##### Summary

number of vertices

<a name='F-H3Lib-GeoBoundary-Verts'></a>
### Verts `constants`

##### Summary

vertices in ccw order

<a name='M-H3Lib-GeoBoundary-ToString'></a>
### ToString() `method`

##### Summary

Debug information in string form

##### Parameters

This method has no parameters.

<a name='T-H3Lib-GeoCoord'></a>
## GeoCoord `type`

##### Namespace

H3Lib

##### Summary

Functions for working with lat/lon coordinates.

<a name='M-H3Lib-GeoCoord-#ctor-System-Double,System-Double-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-GeoCoord-#ctor-H3Lib-GeoCoord-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-GeoCoord-Latitude'></a>
### Latitude `constants`

##### Summary

Latitude normally in radians

<a name='F-H3Lib-GeoCoord-Longitude'></a>
### Longitude `constants`

##### Summary

Longitude normally in radians

<a name='M-H3Lib-GeoCoord-EdgeLengthKm-System-Int32-'></a>
### EdgeLengthKm() `method`

##### Summary

Length of cell edge at resolution in kilometers

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-EdgeLengthM-System-Int32-'></a>
### EdgeLengthM() `method`

##### Summary

Length of cell edge at resolution in meters

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-Equals-H3Lib-GeoCoord-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality test against unboxed object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-HexAreaKm2-System-Int32-'></a>
### HexAreaKm2() `method`

##### Summary

Area of cell at resolution in square kilometers

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-HexAreaM2-System-Int32-'></a>
### HexAreaM2() `method`

##### Summary

Area of cell at resolution in square meters

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-ToString'></a>
### ToString() `method`

##### Summary

Debug information in string form

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-TriangleArea-H3Lib-GeoCoord,H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### TriangleArea(a,b,c) `method`

##### Summary

Compute area in radians^2 of a spherical triangle, given its vertices.

##### Returns

area of triangle on unit sphere, in radians^2

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | vertex lat/lng in radians |
| b | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | vertex lat/lng in radians |
| c | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | vertex lat/lng in radians |

<a name='M-H3Lib-GeoCoord-TriangleEdgeLengthToArea-System-Double,System-Double,System-Double-'></a>
### TriangleEdgeLengthToArea(a,b,c) `method`

##### Summary

Surface area in radians^2 of spherical triangle on unit sphere.

 For the math, see:
 https://en.wikipedia.org/wiki/Spherical_trigonometry#Area_and_spherical_excess

##### Returns

area in radians^2 of triangle on unit sphere

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | length of triangle side A in radians |
| b | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | length of triangle side B in radians |
| c | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | length of triangle side C in radians |

<a name='M-H3Lib-GeoCoord-op_Equality-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### op_Equality() `method`

##### Summary

Equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-GeoCoord-op_Inequality-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### op_Inequality() `method`

##### Summary

Inequality operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-GeoCoordExtensions'></a>
## GeoCoordExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations for GeoCoord type

<a name='M-H3Lib-Extensions-GeoCoordExtensions-AzimuthRadiansTo-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### AzimuthRadiansTo(p1,p2) `method`

##### Summary

Determines the azimuth to p2 from p1 in radians

##### Returns

The azimuth in radians from p1 to p2

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| p1 | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The first spherical coordinates |
| p2 | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The second spherical coordinates |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-DistanceToKm-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### DistanceToKm(a,b) `method`

##### Summary

The great circle distance in kilometers between two spherical coordinates

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the first lat/lng pair (in radians) |
| b | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the second lat/lng pair (in radians) |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-DistanceToM-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### DistanceToM(a,b) `method`

##### Summary

The great circle distance in meters between two spherical coordinates

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the first lat/lng pair (in radians) |
| b | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the second lat/lng pair (in radians) |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-DistanceToRadians-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### DistanceToRadians(a,b) `method`

##### Summary

The great circle distance in radians between two spherical coordinates.
This function uses the Haversine formula.
For math details, see:
    https://en.wikipedia.org/wiki/Haversine_formula
    https://www.movable-type.co.uk/scripts/latlong.html

##### Returns

the great circle distance in radians between a and b

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| a | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the first lat/lng pair (in radians) |
| b | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the second lat/lng pair (in radians) |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-GetAzimuthDistancePoint-H3Lib-GeoCoord,System-Double,System-Double-'></a>
### GetAzimuthDistancePoint(p1,azimuth,distance) `method`

##### Summary

Computes the point on the sphere a specified azimuth and distance from
another point.

##### Returns

The spherical coordinates at the desired azimuth and distance from p1.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| p1 | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The first spherical coordinates. |
| azimuth | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired azimuth from p1. |
| distance | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired distance from p1, must be non-negative. |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-LineHexEstimate-H3Lib-GeoCoord,H3Lib-GeoCoord,System-Int32-'></a>
### LineHexEstimate(origin,destination,res) `method`

##### Summary

returns an estimated number of hexagons that trace
the cartesian-projected line

##### Returns

the estimated number of hexagons required to trace the line

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the origin coordinates |
| destination | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | the destination coordinates |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | the resolution of the H3 hexagons to trace the line |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-SetDegrees-H3Lib-GeoCoord,System-Double,System-Double-'></a>
### SetDegrees(gc,latitude,longitude) `method`

##### Summary

Set the components of spherical coordinates in decimal degrees.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| gc | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The spherical coordinates |
| latitude | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired latitude in decimal degrees |
| longitude | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired longitude in decimal degrees |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-SetGeoRads-H3Lib-GeoCoord,System-Double,System-Double-'></a>
### SetGeoRads(gc,latitudeRadians,longitudeRadians) `method`

##### Summary

Set the components of spherical coordinates in radians.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| gc | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The spherical coordinates |
| latitudeRadians | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired latitude in decimal radians |
| longitudeRadians | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired longitude in decimal radians |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-SetLatitude-H3Lib-GeoCoord,System-Double-'></a>
### SetLatitude() `method`

##### Summary

Quick replacement for Latitude

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-GeoCoordExtensions-SetLongitude-H3Lib-GeoCoord,System-Double-'></a>
### SetLongitude() `method`

##### Summary

Quick replacement for Longitude

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-GeoCoordExtensions-SetRadians-H3Lib-GeoCoord,System-Double,System-Double-'></a>
### SetRadians(gc,latitude,longitude) `method`

##### Summary

Set the components of spherical coordinates in radians.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| gc | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The spherical coordinates |
| latitude | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired latitude in decimal radians |
| longitude | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The desired longitude in decimal radians |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-ToFaceIjk-H3Lib-GeoCoord,System-Int32-'></a>
### ToFaceIjk(g,res) `method`

##### Summary

Encodes a coordinate on the sphere to the FaceIJK address of the containing
cell at the specified resolution.

##### Returns

The FaceIJK address of the containing cell at resolution res.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| g | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The spherical coordinates to encode. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The desired H3 resolution for the encoding. |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-ToH3Index-H3Lib-GeoCoord,System-Int32-'></a>
### ToH3Index(g,res) `method`

##### Summary

Encodes a coordinate on the sphere to the H3 index of the containing cell at
 the specified resolution.

 Returns 0 on invalid input.

##### Returns

The encoded H3Index (or H3_NULL on failure).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| g | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The spherical coordinates to encode. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The desired H3 resolution for the encoding. |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-ToHex2d-H3Lib-GeoCoord,System-Int32-'></a>
### ToHex2d(g,res) `method`

##### Summary

Encodes a coordinate on the sphere to the corresponding icosahedral face and
containing 2D hex coordinates relative to that face center.

##### Returns

Tuple
Item1: The resulting face
Item2: The 2D hex coordinates of the cell containing the point.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| g | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The spherical coordinates to encode. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The desired H3 resolution for the encoding. |

<a name='M-H3Lib-Extensions-GeoCoordExtensions-ToVec3d-H3Lib-GeoCoord-'></a>
### ToVec3d(geo) `method`

##### Summary

Calculate the 3D coordinate on unit sphere from the latitude and longitude.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| geo | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The latitude and longitude of the point |

<a name='T-H3Lib-GeoFence'></a>
## GeoFence `type`

##### Namespace

H3Lib

##### Summary

similar to GeoBoundary, but requires more alloc work

<a name='M-H3Lib-GeoFence-#ctor'></a>
### #ctor() `constructor`

##### Summary

constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-GeoFence-NumVerts'></a>
### NumVerts `constants`

##### Summary

number of vertices

<a name='F-H3Lib-GeoFence-Verts'></a>
### Verts `constants`

##### Summary

vertices in ccw order

<a name='P-H3Lib-GeoFence-IsEmpty'></a>
### IsEmpty `property`

##### Summary

Indicates if the geofence has no vertices

<a name='T-H3Lib-Extensions-GeoFenceExtensions'></a>
## GeoFenceExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations for GeoFence type

<a name='M-H3Lib-Extensions-GeoFenceExtensions-GetEdgeHexagons-H3Lib-GeoFence,System-Int32,System-Int32,System-Int32@,System-Collections-Generic-List{H3Lib-H3Index}@,System-Collections-Generic-List{H3Lib-H3Index}@-'></a>
### GetEdgeHexagons(geofence,numHexagons,res,numSearchHexagons,search,found) `method`

##### Summary

_getEdgeHexagons takes a given geofence ring (either the main geofence or
 one of the holes) and traces it with hexagons and updates the search and
 found memory blocks. This is used for determining the initial hexagon set
 for the polyfill algorithm to execute on.

##### Returns

An error code if the hash function cannot insert a found hexagon into the found array.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| geofence | [H3Lib.GeoFence](#T-H3Lib-GeoFence 'H3Lib.GeoFence') | The geofence (or hole) to be traced |
| numHexagons | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The maximum number of hexagons possible for the geofence
 (also the bounds of the search and found arrays) |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The hexagon resolution (0-15) |
| numSearchHexagons | [System.Int32@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32@ 'System.Int32@') | The number of hexagons found so far to be searched |
| search | [System.Collections.Generic.List{H3Lib.H3Index}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}@') | The block of memory containing the hexagons to search from |
| found | [System.Collections.Generic.List{H3Lib.H3Index}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}@') | The block of memory containing the hexagons found from the search |

<a name='M-H3Lib-Extensions-GeoFenceExtensions-IsClockwise-H3Lib-GeoFence-'></a>
### IsClockwise(loop) `method`

##### Summary

Is GeoFence clockwise?

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.GeoFence](#T-H3Lib-GeoFence 'H3Lib.GeoFence') |  |

<a name='M-H3Lib-Extensions-GeoFenceExtensions-IsClockwiseNormalized-H3Lib-GeoFence,System-Boolean-'></a>
### IsClockwiseNormalized(loop,isTransmeridian) `method`

##### Summary

Is loop clockwise oriented?

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.GeoFence](#T-H3Lib-GeoFence 'H3Lib.GeoFence') |  |
| isTransmeridian | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-H3Lib-Extensions-GeoFenceExtensions-PointInside-H3Lib-GeoFence,H3Lib-BBox,H3Lib-GeoCoord-'></a>
### PointInside(loop,box,coord) `method`

##### Summary

Is boint within the loo?

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.GeoFence](#T-H3Lib-GeoFence 'H3Lib.GeoFence') |  |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') |  |
| coord | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |

<a name='M-H3Lib-Extensions-GeoFenceExtensions-ToBBox-H3Lib-GeoFence-'></a>
### ToBBox(loop) `method`

##### Summary

Create a bounding box from a simple polygon loop

##### Returns

output box

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.GeoFence](#T-H3Lib-GeoFence 'H3Lib.GeoFence') | Loop of coordinates |

##### Remarks

Known limitations:
- Does not support polygons with two adjacent points > 180 degrees of
  longitude apart. These will be interpreted as crossing the antimeridian.
- Does not currently support polygons containing a pole.

<a name='T-H3Lib-GeoMultiPolygon'></a>
## GeoMultiPolygon `type`

##### Namespace

H3Lib

##### Summary

Simplified core of GeoJSON MultiPolygon coordinates definition

<a name='F-H3Lib-GeoMultiPolygon-NumPolygons'></a>
### NumPolygons `constants`

##### Summary

Number of elements in the array pointed to by the holes

<a name='F-H3Lib-GeoMultiPolygon-Polygons'></a>
### Polygons `constants`

##### Summary

interior boundaries (holes) in the polygon

<a name='T-H3Lib-GeoPolygon'></a>
## GeoPolygon `type`

##### Namespace

H3Lib

##### Summary

Simplified core of GeoJSON Polygon coordinates definition

<a name='F-H3Lib-GeoPolygon-GeoFence'></a>
### GeoFence `constants`

##### Summary

exterior boundary of the polygon

<a name='F-H3Lib-GeoPolygon-Holes'></a>
### Holes `constants`

##### Summary

interior boundaries (holes) in the polygon

<a name='F-H3Lib-GeoPolygon-NumHoles'></a>
### NumHoles `constants`

##### Summary

Number of elements in the array pointed to by the holes

<a name='T-H3Lib-Extensions-GeoPolygonExtensions'></a>
## GeoPolygonExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations on GeoPolygon type

<a name='M-H3Lib-Extensions-GeoPolygonExtensions-MaxPolyFillSize-H3Lib-GeoPolygon,System-Int32-'></a>
### MaxPolyFillSize(geoPolygon,res) `method`

##### Summary

maxPolyfillSize returns the number of hexagons to allocate space for when
 performing a polyfill on the given GeoJSON-like data structure.

 The size is the maximum of either the number of points in the geofence or the
 number of hexagons in the bounding box of the geofence.

##### Returns

number of hexagons to allocate for

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| geoPolygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') | A GeoJSON-like data structure indicating the poly to fill |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Hexagon resolution (0-15) |

<a name='M-H3Lib-Extensions-GeoPolygonExtensions-PointInside-H3Lib-GeoPolygon,System-Collections-Generic-List{H3Lib-BBox},H3Lib-GeoCoord-'></a>
### PointInside(polygon,boxes,coord) `method`

##### Summary

pointInsidePolygon takes a given GeoPolygon data structure and
checks if it contains a given geo coordinate.

##### Returns

Whether the point is contained

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| polygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') | The geofence and holes defining the relevant area |
| boxes | [System.Collections.Generic.List{H3Lib.BBox}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.BBox}') | The bboxes for the main geofence and each of its holes |
| coord | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | The coordinate to check |

<a name='M-H3Lib-Extensions-GeoPolygonExtensions-PolyFillInternal-H3Lib-GeoPolygon,System-Int32-'></a>
### PolyFillInternal(geoPolygon,res) `method`

##### Summary

_polyfillInternal traces the provided geoPolygon data structure with hexagons
and then iteratively searches through these hexagons and their immediate
neighbors to see if they are contained within the polygon or not. Those that
are found are added to the out array as well as the found array. Once all
hexagons to search are checked, the found hexagons become the new search
array and the found array is wiped and the process repeats until no new
hexagons can be found.

##### Returns

Tuple
Item1 - Status code
Item2 - List of H3Index values

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| geoPolygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') | The geofence and holes defining the relevant area |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The Hexagon resolution (0-15) |

<a name='M-H3Lib-Extensions-GeoPolygonExtensions-Polyfill-H3Lib-GeoPolygon,System-Int32-'></a>
### Polyfill(polygon,res) `method`

##### Summary

polyfill takes a given GeoJSON-like data structure and preallocated,
 zeroed memory, and fills it with the hexagons that are contained by
 the GeoJSON-like data structure.

 This implementation traces the GeoJSON geofence(s) in cartesian space with
 hexagons, tests them and their neighbors to be contained by the geofence(s),
 and then any newly found hexagons are used to test again until no new
 hexagons are found.

##### Returns

List of H3Index that compose the polyfill

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| polygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') | The geofence and holes defining the relevant area |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The Hexagon resolution (0-15) |

<a name='M-H3Lib-Extensions-GeoPolygonExtensions-ToBBoxes-H3Lib-GeoPolygon-'></a>
### ToBBoxes(polygon) `method`

##### Summary

Create a set of bounding boxes from a GeoPolygon

##### Returns

Output bboxes, one for the outer loop and one for each hole

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| polygon | [H3Lib.GeoPolygon](#T-H3Lib-GeoPolygon 'H3Lib.GeoPolygon') | Input GeoPolygon |

<a name='T-H3Lib-Constants-H3'></a>
## H3 `type`

##### Namespace

H3Lib.Constants

##### Summary

Internal values for all of H3

<a name='F-H3Lib-Constants-H3-DBL_EPSILON'></a>
### DBL_EPSILON `constants`

##### Summary

General margin of error in differences between double values
 Original value was 2.2204460492503131e-16;

 This is relevant to accuracy's interest, methinks
 https://github.com/dotnet/runtime/issues/8528

<a name='F-H3Lib-Constants-H3-EARTH_RADIUS_KM'></a>
### EARTH_RADIUS_KM `constants`

##### Summary

Earth radius in kilometers using WGS84 authalic radius

<a name='F-H3Lib-Constants-H3-EPSILON'></a>
### EPSILON `constants`

##### Summary

Threshold epsilon

<a name='F-H3Lib-Constants-H3-EPSILON_DEG'></a>
### EPSILON_DEG `constants`

##### Summary

epsilon of ~0.1mm in degrees

<a name='F-H3Lib-Constants-H3-EPSILON_RAD'></a>
### EPSILON_RAD `constants`

##### Summary

epsilon of ~0.1mm in radians

<a name='F-H3Lib-Constants-H3-H3_HEXAGON_MODE'></a>
### H3_HEXAGON_MODE `constants`

##### Summary

H3 Index modes

<a name='F-H3Lib-Constants-H3-MAX_H3_RES'></a>
### MAX_H3_RES `constants`

##### Summary

H3 resolution; H3 version 1 has 16 resolutions, numbered 0 through 15

<a name='F-H3Lib-Constants-H3-M_180_PI'></a>
### M_180_PI `constants`

##### Summary

180 / Pi

<a name='F-H3Lib-Constants-H3-M_2PI'></a>
### M_2PI `constants`

##### Summary

Pi * 2.0

<a name='F-H3Lib-Constants-H3-M_AP7_ROT_RADS'></a>
### M_AP7_ROT_RADS `constants`

##### Summary

Rotation angle between Class II and Class III resolution axes
asin(sqrt(3.0 / 28.0 ))

<a name='F-H3Lib-Constants-H3-M_COS_AP7_ROT'></a>
### M_COS_AP7_ROT `constants`

##### Summary

cos([M_AP7_ROT_RADS](#F-H3Lib-Constants-H3-M_AP7_ROT_RADS 'H3Lib.Constants.H3.M_AP7_ROT_RADS')

<a name='F-H3Lib-Constants-H3-M_PI'></a>
### M_PI `constants`

##### Summary

Pi

<a name='F-H3Lib-Constants-H3-M_PI_180'></a>
### M_PI_180 `constants`

##### Summary

Pi / 180

<a name='F-H3Lib-Constants-H3-M_PI_2'></a>
### M_PI_2 `constants`

##### Summary

Pi / 2.0

<a name='F-H3Lib-Constants-H3-M_SIN60'></a>
### M_SIN60 `constants`

##### Summary

sin(60 degrees)

<a name='F-H3Lib-Constants-H3-M_SIN_AP7_ROT'></a>
### M_SIN_AP7_ROT `constants`

##### Summary

sin([M_AP7_ROT_RADS](#F-H3Lib-Constants-H3-M_AP7_ROT_RADS 'H3Lib.Constants.H3.M_AP7_ROT_RADS')

<a name='F-H3Lib-Constants-H3-M_SQRT3_2'></a>
### M_SQRT3_2 `constants`

##### Summary

Sqrt(3) / 2.0

<a name='F-H3Lib-Constants-H3-NEXT_RING_DIRECTION'></a>
### NEXT_RING_DIRECTION `constants`

##### Summary

Direction used for traversing to the next outward hexagonal ring.

<a name='F-H3Lib-Constants-H3-NUM_BASE_CELLS'></a>
### NUM_BASE_CELLS `constants`

##### Summary

The number of H3 base cells

<a name='F-H3Lib-Constants-H3-NUM_HEX_VERTS'></a>
### NUM_HEX_VERTS `constants`

##### Summary

The number of vertices in a hexagon;

<a name='F-H3Lib-Constants-H3-NUM_ICOSA_FACES'></a>
### NUM_ICOSA_FACES `constants`

##### Summary

The number of faces on an icosahedron

<a name='F-H3Lib-Constants-H3-NUM_PENT_VERTS'></a>
### NUM_PENT_VERTS `constants`

##### Summary

The number of vertices in a pentagon

<a name='F-H3Lib-Constants-H3-RES0_U_GNOMONIC'></a>
### RES0_U_GNOMONIC `constants`

##### Summary

Scaling factor from hex2d resolution 0 unit length
(or distance between adjacent cell center points on the place)
to gnomonic unit length.

<a name='T-H3Lib-Constants-H3Index'></a>
## H3Index `type`

##### Namespace

H3Lib.Constants

<a name='T-H3Lib-H3Index'></a>
## H3Index `type`

##### Namespace

H3Lib

##### Summary

H3Index utility functions

<a name='M-H3Lib-H3Index-#ctor-System-UInt64-'></a>
### #ctor(val) `constructor`

##### Summary

Constructor

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| val | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') |  |

<a name='M-H3Lib-H3Index-#ctor-System-Int32,System-Int32,H3Lib-Direction-'></a>
### #ctor() `constructor`

##### Summary

constructor

This came about from getting tired of two stepping
constructors for unit tests.

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-H3Index-#ctor-System-Int32,System-Int32,System-Int32-'></a>
### #ctor(res,baseCell,initDigit) `constructor`

##### Summary

constructor

This came about from getting tired of two stepping
constructors for unit tests.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| baseCell | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| initDigit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='F-H3Lib-Constants-H3Index-H3_BC_MASK'></a>
### H3_BC_MASK `constants`

##### Summary

1's in the 7 base cell bits, 0's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_BC_MASK_NEGATIVE'></a>
### H3_BC_MASK_NEGATIVE `constants`

##### Summary

0's in the 7 base cell bits, 1's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_BC_OFFSET'></a>
### H3_BC_OFFSET `constants`

##### Summary

The bit offset of the base cell in an H3 index.

<a name='F-H3Lib-Constants-H3Index-H3_DIGIT_MASK'></a>
### H3_DIGIT_MASK `constants`

##### Summary

1's in the 3 bits of res 15 digit bits, 0's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_DIGIT_MASK_NEGATIVE'></a>
### H3_DIGIT_MASK_NEGATIVE `constants`

##### Summary

0's in the 7 base cell bits, 1's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_HIGH_BIT_MASK'></a>
### H3_HIGH_BIT_MASK `constants`

##### Summary

1 in the highest bit, 0's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_HIGH_BIT_MASK_NEGATIVE'></a>
### H3_HIGH_BIT_MASK_NEGATIVE `constants`

##### Summary

0 in the highest bit, 1's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_INIT'></a>
### H3_INIT `constants`

<a name='F-H3Lib-Constants-H3Index-H3_INVALID_INDEX'></a>
### H3_INVALID_INDEX `constants`

##### Summary

Invalid index used to indicate an error from geoToH3 and related functions.

<a name='F-H3Lib-Constants-H3Index-H3_MAX_OFFSET'></a>
### H3_MAX_OFFSET `constants`

##### Summary

The bit offset of the max resolution digit in an H3 index.

<a name='F-H3Lib-Constants-H3Index-H3_MODE_MASK'></a>
### H3_MODE_MASK `constants`

##### Summary

1's in the 4 mode bits, 0's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_MODE_MASK_NEGATIVE'></a>
### H3_MODE_MASK_NEGATIVE `constants`

##### Summary

0's in the 4 mode bits, 1's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_MODE_OFFSET'></a>
### H3_MODE_OFFSET `constants`

##### Summary

The bit offset of the mode in an H3 index.

<a name='F-H3Lib-Constants-H3Index-H3_NULL'></a>
### H3_NULL `constants`

##### Summary

Invalid index used to indicate an error from geoToH3 and related functions
or missing data in arrays of h3 indices. Analogous to NaN in floating point.

<a name='F-H3Lib-Constants-H3Index-H3_NUM_BITS'></a>
### H3_NUM_BITS `constants`

##### Summary

The number of bits in an H3 index.

<a name='F-H3Lib-Constants-H3Index-H3_PER_DIGIT_OFFSET'></a>
### H3_PER_DIGIT_OFFSET `constants`

##### Summary

The number of bits in a single H3 resolution digit.

<a name='F-H3Lib-Constants-H3Index-H3_RESERVED_MASK'></a>
### H3_RESERVED_MASK `constants`

##### Summary

1's in the 3 reserved bits, 0's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_RESERVED_MASK_NEGATIVE'></a>
### H3_RESERVED_MASK_NEGATIVE `constants`

##### Summary

0's in the 3 reserved bits, 1's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_RESERVED_OFFSET'></a>
### H3_RESERVED_OFFSET `constants`

##### Summary

The bit offset of the reserved bits in an H3 index.

<a name='F-H3Lib-Constants-H3Index-H3_RES_MASK'></a>
### H3_RES_MASK `constants`

##### Summary

1's in the 4 resolution bits, 0's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_RES_MASK_NEGATIVE'></a>
### H3_RES_MASK_NEGATIVE `constants`

##### Summary

0's in the 4 resolution bits, 1's everywhere else.

<a name='F-H3Lib-Constants-H3Index-H3_RES_OFFSET'></a>
### H3_RES_OFFSET `constants`

##### Summary

The bit offset of the resolution in an H3 index.

<a name='F-H3Lib-H3Index-Value'></a>
### Value `constants`

##### Summary

Where the actual index is stored.

<a name='P-H3Lib-H3Index-BaseCell'></a>
### BaseCell `property`

##### Summary

Integer base cell of H3

<a name='P-H3Lib-H3Index-HighBit'></a>
### HighBit `property`

##### Summary

High bit of H3

<a name='P-H3Lib-H3Index-IsResClassIii'></a>
### IsResClassIii `property`

##### Summary

IsResClassIII takes a hexagon ID and determines if it is in a
Class III resolution (rotated versus the icosahedron and subject
to shape distortion adding extra points on icosahedron edges, making
them not true hexagons).

<a name='P-H3Lib-H3Index-LeadingNonZeroDigit'></a>
### LeadingNonZeroDigit `property`

##### Summary

Returns the highest resolution non-zero digit in an H3Index.

<a name='P-H3Lib-H3Index-Mode'></a>
### Mode `property`

##### Summary

Integer mode of H3

<a name='P-H3Lib-H3Index-PentagonIndexCount'></a>
### PentagonIndexCount `property`

##### Summary

returns the number of pentagons (same at any resolution)

<a name='P-H3Lib-H3Index-ReservedBits'></a>
### ReservedBits `property`

##### Summary

Reserved bits of H3Index

<a name='P-H3Lib-H3Index-Resolution'></a>
### Resolution `property`

##### Summary

Integer resolution of an H3 index.

<a name='M-H3Lib-H3Index-CompareTo-H3Lib-H3Index-'></a>
### CompareTo() `method`

##### Summary

Compare test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-Equals-H3Lib-H3Index-'></a>
### Equals(other) `method`

##### Summary

Equality test

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| other | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') |  |

<a name='M-H3Lib-H3Index-Equals-System-UInt64-'></a>
### Equals() `method`

##### Summary

Equal against ulong

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equal test against object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-GetIndexDigit-System-Int32-'></a>
### GetIndexDigit() `method`

##### Summary

Gets the resolution res integer digit (0-7) of h3.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-ToString'></a>
### ToString() `method`

##### Summary

Converts an H3 index into a string representation.

##### Returns

The string representation of the H3 index as a hexadecimal number

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-op_Equality-H3Lib-H3Index,H3Lib-H3Index-'></a>
### op_Equality() `method`

##### Summary

Equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-op_Implicit-System-UInt64-~H3Lib-H3Index'></a>
### op_Implicit() `method`

##### Summary

Implicit conversion

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-op_Implicit-H3Lib-H3Index-~System-UInt64'></a>
### op_Implicit() `method`

##### Summary

Implicit conversion

##### Parameters

This method has no parameters.

<a name='M-H3Lib-H3Index-op_Inequality-H3Lib-H3Index,H3Lib-H3Index-'></a>
### op_Inequality() `method`

##### Summary

Inequality operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-H3IndexExtensions'></a>
## H3IndexExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations that act upon a data type of [H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') located
in one central location.

<a name='M-H3Lib-Extensions-H3IndexExtensions-CellAreaKm2-H3Lib-H3Index-'></a>
### CellAreaKm2(h) `method`

##### Summary

Area of H3 cell in kilometers^2.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | h3 cell |

<a name='M-H3Lib-Extensions-H3IndexExtensions-CellAreaM2-H3Lib-H3Index-'></a>
### CellAreaM2(h) `method`

##### Summary

Area of H3 cell in meters^2.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | h3 cell |

<a name='M-H3Lib-Extensions-H3IndexExtensions-CellAreaRadians2-H3Lib-H3Index-'></a>
### CellAreaRadians2(cell) `method`

##### Summary

Area of H3 cell in radians^2.

 The area is calculated by breaking the cell into spherical triangles and
 summing up their areas. Note that some H3 cells (hexagons and pentagons)
 are irregular, and have more than 6 or 5 sides.

 todo: optimize the computation by re-using the edges shared between triangles

##### Returns

cell area in radians^2

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cell | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3 cell |

<a name='M-H3Lib-Extensions-H3IndexExtensions-DestinationFromUniDirectionalEdge-H3Lib-H3Index-'></a>
### DestinationFromUniDirectionalEdge(edge) `method`

##### Summary

Returns the destination hexagon from the unidirectional edge H3Index

##### Returns

The destination H3 hexagon index, or H3_NULL on failure

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The edge H3 index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-DistanceTo-H3Lib-H3Index,H3Lib-H3Index-'></a>
### DistanceTo(origin,h3) `method`

##### Summary

Produces the grid distance between the two indexes.

This function may fail to find the distance between two indexes, for
example if they are very far apart. It may also fail when finding
distances for indexes on opposite sides of a pentagon.

##### Returns

The distance, or a negative number if the library could not
compute the distance.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Index to find the distance from. |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Index to find the distance to. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ExactEdgeLengthKm-H3Lib-H3Index-'></a>
### ExactEdgeLengthKm(edge) `method`

##### Summary

Length of a unidirectional edge in kilometers.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3 unidirectional edge |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ExactEdgeLengthM-H3Lib-H3Index-'></a>
### ExactEdgeLengthM(edge) `method`

##### Summary

Length of a unidirectional edge in meters.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3 unidirectional edge |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ExactEdgeLengthRads-H3Lib-H3Index-'></a>
### ExactEdgeLengthRads(edge) `method`

##### Summary

Length of a unidirectional edge in radians.

##### Returns

length in radians

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3 unidirectional edge |

<a name='M-H3Lib-Extensions-H3IndexExtensions-GetFaces-H3Lib-H3Index-'></a>
### GetFaces(h3) `method`

##### Summary

Find all icosahedron faces intersected by a given H3 index, represented
as integers from 0-19. The array is sparse; since 0 is a valid value,
invalid array values are represented as -1. It is the responsibility of
the caller to filter out invalid values.

##### Returns

Output list.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3 index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-GetH3IndexesArrayFromUniEdge-H3Lib-H3Index-'></a>
### GetH3IndexesArrayFromUniEdge(edge) `method`

##### Summary

Returns the origin, destination pair of hexagon IDs for the given edge ID

##### Returns

Tuple containing origin and destination H#Index cells of edge

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The unidirectional edge H3Index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-GetH3IndexesFromUniEdge-H3Lib-H3Index-'></a>
### GetH3IndexesFromUniEdge(edge) `method`

##### Summary

Returns the origin, destination pair of hexagon IDs for the given edge ID

##### Returns

Tuple containing origin and destination H#Index cells of edge

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The unidirectional edge H3Index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-GetUniEdgesFromCell-H3Lib-H3Index-'></a>
### GetUniEdgesFromCell(origin) `method`

##### Summary

Provides all of the unidirectional edges from the current H3Index.

##### Returns

List of edges

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The origin hexagon H3Index to find edges for. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-HexRadiusKm-H3Lib-H3Index-'></a>
### HexRadiusKm(h3) `method`

##### Summary

returns the radius of a given hexagon in Km

##### Returns

the radius of the hexagon in Km

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | the index of the hexagon |

<a name='M-H3Lib-Extensions-H3IndexExtensions-HexRange-H3Lib-H3Index,System-Int32-'></a>
### HexRange(origin,k) `method`

##### Summary

hexRange produces indexes within k distance of the origin index.

 Output behavior is undefined when one of the indexes returned by this
 function is a pentagon or is in the pentagon distortion area.

 k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
 all neighboring indexes, and so on.

 Output is placed in the provided array in order of increasing distance from
 the origin.

##### Returns

Tuple
     Item1 - 0 if no pentagon or pentagonal distortion area was encountered.
     Item2 - List of H3Index cells

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Origin location. |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k >= 0 |

<a name='M-H3Lib-Extensions-H3IndexExtensions-HexRangeDistances-H3Lib-H3Index,System-Int32-'></a>
### HexRangeDistances(origin,k) `method`

##### Summary

Produces indexes within k distance of the origin index.
 Output behavior is undefined when one of the indexes returned by this
 function is a pentagon or is in the pentagon distortion area.

 k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
 all neighboring indexes, and so on.

 Output is placed in the provided array in order of increasing distance from
 the origin. The distances in hexagons is placed in the distances array at
 the same offset.

##### Returns

Tuple with list of tuples
 Main tuple:
     Item1 : status code
     Item2 : List of tuples
 Item1: H3Index
 Item2: distance

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Origin location. |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k >= 0 |

<a name='M-H3Lib-Extensions-H3IndexExtensions-HexRing-H3Lib-H3Index,System-Int32-'></a>
### HexRing(origin,k) `method`

##### Summary

Returns the "hollow" ring of hexagons at exactly grid distance k from
 the origin hexagon. In particular, k=0 returns just the origin hexagon.

 A nonzero failure code may be returned in some cases, for example,
 if a pentagon is encountered.

 Failure cases may be fixed in future versions.

##### Returns

Tuple
     Item1 - Status: 0 if successful, other if failure
     Item2 - List of h3index cells if status == 0, otherwise empty list

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Origin location. |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k >= 0 |

<a name='M-H3Lib-Extensions-H3IndexExtensions-IsNeighborTo-H3Lib-H3Index,H3Lib-H3Index-'></a>
### IsNeighborTo(origin,destination) `method`

##### Summary

Returns whether or not the provided H3Indexes are neighbors.

##### Returns

true if the indices are neighbors, false otherwise

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The origin H3 index. |
| destination | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The destination H3 index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-IsPentagon-H3Lib-H3Index-'></a>
### IsPentagon(h) `method`

##### Summary

Takes an H3Index and determines if it is actually a pentagon.

##### Returns

Returns true if it is a pentagon, otherwise false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3Index to check. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-IsValid-H3Lib-H3Index-'></a>
### IsValid(h) `method`

##### Summary

Returns whether or not an H3 index is a valid cell (hexagon or pentagon).

##### Returns

true if the H3 index is valid

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3 index to validate. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-IsValidUniEdge-H3Lib-H3Index-'></a>
### IsValidUniEdge(edge) `method`

##### Summary

Determines if the provided H3Index is a valid unidirectional edge index

##### Returns

true if it is a unidirectional edge H3Index, otherwise false

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The unidirectional edge H3Index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-KRing-H3Lib-H3Index,System-Int32-'></a>
### KRing(origin,k) `method`

##### Summary

Produce cells within grid distance k of the origin cell.

 k-ring 0 is defined as the origin cell, k-ring 1 is defined as k-ring 0 and
 all neighboring cells, and so on.
 
 Output is placed in the provided array in no particular order. Elements of
 the output array may be left zero, as can happen when crossing a pentagon.

##### Returns

H3Index cells within range

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | origin cell |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k >= 0 |

<a name='M-H3Lib-Extensions-H3IndexExtensions-KRingDistances-H3Lib-H3Index,System-Int32-'></a>
### KRingDistances(origin,k) `method`

##### Summary

Produce cells and their distances from the given origin cell, up to
 distance k

 k-ring 0 is defined as the origin cell, k-ring 1 is defined as k-ring 0 and
 all neighboring cells, and so on.

 Output is placed in the provided array in no particular order. Elements of
 the output array may be left zero, as can happen when crossing a pentagon.

##### Returns

A dictionary with keys being the H3Index, and the value being the distance.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | origin cell |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k >= 0 |

<a name='M-H3Lib-Extensions-H3IndexExtensions-KRingInternal-H3Lib-H3Index,System-Int32,System-Int32,System-Collections-Generic-Dictionary{H3Lib-H3Index,System-Int32}-'></a>
### KRingInternal(origin,k,currentK,outData) `method`

##### Summary

Internal helper function called recursively for kRingDistances.

Adds the origin cell to the output set (treating it as a hash set)
and recurses to its neighbors, if needed.

##### Returns

Dictionary of cells
Key - element either an H3Index or 0
Value - indicate ijk distance from the origin cell to Item2

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Origin cell |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Maximum distance to move from the origin |
| currentK | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Current distance from the origin |
| outData | [System.Collections.Generic.Dictionary{H3Lib.H3Index,System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{H3Lib.H3Index,System.Int32}') | Dictionary passing information between recursions |

##### Remarks

NOTE: You _should_ be able to just call this with h3.KRingInternal(k).  We'll see.

<a name='M-H3Lib-Extensions-H3IndexExtensions-LineSize-H3Lib-H3Index,H3Lib-H3Index-'></a>
### LineSize(start,end) `method`

##### Summary

Number of indexes in a line from the start index to the end index,
to be used for allocating memory. Returns a negative number if the
line cannot be computed.

##### Returns

Size of the line, or a negative number if the line cannot
be computed.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| start | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Start index of the line |
| end | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | End index of the line |

<a name='M-H3Lib-Extensions-H3IndexExtensions-LineTo-H3Lib-H3Index,H3Lib-H3Index-'></a>
### LineTo(start,end) `method`

##### Summary

Given two H3 indexes, return the line of indexes between them (inclusive).

 This function may fail to find the line between two indexes, for
 example if they are very far apart. It may also fail when finding
 distances for indexes on opposite sides of a pentagon.

 Notes:
  - The specific output of this function should not be considered stable
    across library versions. The only guarantees the library provides are
    that the line length will be \`h3Distance(start, end) + 1\` and that
    every index in the line will be a neighbor of the preceding index.
  - Lines are drawn in grid space, and may not correspond exactly to either
    Cartesian lines or great arcs.

##### Returns

Tuple:
 (status, IEnumerable)
 status => 0 success, otherwise failure

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| start | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Start index of the line |
| end | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | End index of the line |

<a name='M-H3Lib-Extensions-H3IndexExtensions-MakeDirectChild-H3Lib-H3Index,System-Int32-'></a>
### MakeDirectChild(h,cellNumber) `method`

##### Summary

MakeDirectChild takes an index and immediately returns the immediate child
index based on the specified cell number. Bit operations only, could generate
invalid indexes if not careful (deleted cell under a pentagon).

##### Returns

The new H3Index for the child

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3Index to find the direct child of |
| cellNumber | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | int id of the direct child (0-6) |

<a name='M-H3Lib-Extensions-H3IndexExtensions-MaxChildrenSize-H3Lib-H3Index,System-Int32-'></a>
### MaxChildrenSize(h3,childRes) `method`

##### Summary

MaxChildrenSize returns the maximum number of children possible for a
given child level.

##### Returns

count of maximum number of children (equal for hexagons, less for pentagons

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3Index to find the number of children of |
| childRes | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The resolution of the child level you're interested in |

<a name='M-H3Lib-Extensions-H3IndexExtensions-MaxFaceCount-H3Lib-H3Index-'></a>
### MaxFaceCount(h3) `method`

##### Summary

Returns the max number of possible icosahedron faces an H3 index
may intersect.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') |  |

<a name='M-H3Lib-Extensions-H3IndexExtensions-MaxUncompactSize-H3Lib-H3Index,System-Int32-'></a>
### MaxUncompactSize(singleCell,res) `method`

##### Summary

Lets you get the maxUncompactSize from a single cell instead of
requiring wrapping it in a List

##### Returns

How many hexagons to expect

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| singleCell | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Cell that will be uncompacted |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | resolution to uncompact to |

<a name='M-H3Lib-Extensions-H3IndexExtensions-NeighborRotations-H3Lib-H3Index,H3Lib-Direction,System-Int32-'></a>
### NeighborRotations(origin,dir,rotations) `method`

##### Summary

Returns the hexagon index neighboring the origin, in the direction dir.

 Implementation note: The only reachable case where this returns 0 is if the
 origin is a pentagon and the translation is in the k direction. Thus,
 0 can only be returned if origin is a pentagon.

##### Returns

Tuple
 Item1 - see summary above
 Item2 - Modified rotation value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Origin index |
| dir | [H3Lib.Direction](#T-H3Lib-Direction 'H3Lib.Direction') | Direction to move in |
| rotations | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of ccw rotations to perform to reorient the
 translation vector. Modified version Will be returned in tuple,
 so make sure it's reassigned upon return.  Return will be the
 new number of rotations to perform (such as when crossing a face edge.) |

<a name='M-H3Lib-Extensions-H3IndexExtensions-OriginFromUniDirectionalEdge-H3Lib-H3Index-'></a>
### OriginFromUniDirectionalEdge(edge) `method`

##### Summary

Returns the origin hexagon from the unidirectional edge H3Index

##### Returns

The origin H3 hexagon index, or H3_NULL on failure

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The edge H3 index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-Rotate60Clockwise-H3Lib-H3Index-'></a>
### Rotate60Clockwise(h) `method`

##### Summary

Rotate an H3Index 60 degrees clockwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3Index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-Rotate60CounterClockwise-H3Lib-H3Index-'></a>
### Rotate60CounterClockwise(h) `method`

##### Summary

Rotate an H3Index 60 degrees counter-clockwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3Index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-RotatePent60Clockwise-H3Lib-H3Index-'></a>
### RotatePent60Clockwise(h) `method`

##### Summary

Rotate an H3Index 60 degrees clockwise about a pentagonal center.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3Index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-RotatePent60CounterClockwise-H3Lib-H3Index-'></a>
### RotatePent60CounterClockwise(h) `method`

##### Summary

Rotate an H3Index 60 degrees counter-clockwise about a pentagonal center.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3Index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetBaseCell-H3Lib-H3Index,System-Int32-'></a>
### SetBaseCell() `method`

##### Summary

Set BaseCell of H3Index cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetHighBit-H3Lib-H3Index,System-Int32-'></a>
### SetHighBit() `method`

##### Summary

Sets high bit of H3Index cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetIndex-H3Lib-H3Index,System-Int32,System-Int32,H3Lib-Direction-'></a>
### SetIndex(hp,res,baseCell,initDigit) `method`

##### Summary

Initializes an H3 index.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| hp | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3 index to initialize. |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution to initialize the index to. |
| baseCell | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 base cell to initialize the index to. |
| initDigit | [H3Lib.Direction](#T-H3Lib-Direction 'H3Lib.Direction') | The H3 digit (0-7) to initialize all of the index digits to. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetIndexDigit-H3Lib-H3Index,System-Int32,System-UInt64-'></a>
### SetIndexDigit() `method`

##### Summary

Sets specified index digit of H3Index cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetMode-H3Lib-H3Index,H3Lib-H3Mode-'></a>
### SetMode() `method`

##### Summary

Sets mode of H3Index cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetReservedBits-H3Lib-H3Index,System-Int32-'></a>
### SetReservedBits() `method`

##### Summary

Set reserved bits of H3Index cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-SetResolution-H3Lib-H3Index,System-Int32-'></a>
### SetResolution() `method`

##### Summary

Set resolution of H3Index cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToCenterChild-H3Lib-H3Index,System-Int32-'></a>
### ToCenterChild(h,childRes) `method`

##### Summary

ToCenterChild produces the center child index for a given H3 index at
the specified resolution

##### Returns

H3Index of the center child, or H3_NULL if you actually asked for a parent

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3Index to find center child of |
| childRes | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The resolution to switch to |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToChildren-H3Lib-H3Index,System-Int32-'></a>
### ToChildren(h,childRes) `method`

##### Summary

ToChildren takes the given hexagon id and generates all of the children
at the specified resolution storing them into the provided memory pointer.
It's assumed that maxH3ToChildrenSize was used to determine the allocation.

##### Returns

The list of H3Index children

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3Index to find the children of |
| childRes | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | int the child level to produce |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToFaceIjk-H3Lib-H3Index-'></a>
### ToFaceIjk(h) `method`

##### Summary

Convert an H3Index to a FaceIJK address.

##### Returns

The corresponding FaceIJK address.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3 Index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToFaceIjkWithInitializedFijk-H3Lib-H3Index,H3Lib-FaceIjk-'></a>
### ToFaceIjkWithInitializedFijk(h,fijk) `method`

##### Summary

Convert an H3Index to the FaceIjk address on a specified icosahedral face.

##### Returns

Tuple
Item1: Returns 1 if the possibility of overage exists, otherwise 0.
Item2: Modified FaceIjk

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3Index. |
| fijk | [H3Lib.FaceIjk](#T-H3Lib-FaceIjk 'H3Lib.FaceIjk') | The FaceIjk address, initialized with the desired face
and normalized base cell coordinates. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToGeoBoundary-H3Lib-H3Index-'></a>
### ToGeoBoundary(h3) `method`

##### Summary

Determines the cell boundary in spherical coordinates for an H3 index.

##### Returns

The boundary of the H3 cell in spherical coordinates.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3 index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToGeoCoord-H3Lib-H3Index-'></a>
### ToGeoCoord(h3) `method`

##### Summary

Determines the spherical coordinates of the center point of an H3 index.

##### Returns

The spherical coordinates of the H3 cell center.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The H3 index. |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToLocalIjExperimental-H3Lib-H3Index,H3Lib-H3Index-'></a>
### ToLocalIjExperimental(origin,h3) `method`

##### Summary

Produces ij coordinates for an index anchored by an origin.

 The coordinate space used by this function may have deleted
 regions or warping due to pentagonal distortion.

 Coordinates are only comparable if they come from the same
 origin index.

 Failure may occur if the index is too far away from the origin
 or if the index is on the other side of a pentagon.

 This function is experimental, and its output is not guaranteed
 to be compatible across different versions of H3.

##### Returns

Tuple with Item1 indicating success (0) or other
Item2 contains ij coordinates.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | An anchoring index for the ij coordinate system. |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Index to find the coordinates of |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToLocalIjk-H3Lib-H3Index,H3Lib-H3Index-'></a>
### ToLocalIjk(origin,h3) `method`

##### Summary

Produces ijk+ coordinates for an index anchored by an origin.

 The coordinate space used by this function may have deleted
 regions or warping due to pentagonal distortion.

 Coordinates are only comparable if they come from the same
 origin index.
 
 Failure may occur if the index is too far away from the origin
 or if the index is on the other side of a pentagon.

##### Returns

Item1: 0 on success, or another value on failure.
 Item2: ijk+ coordinates of the index will be placed here on success

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | An anchoring index for the ijk+ coordinate system. |
| h3 | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | Index to find the coordinates of |

<a name='M-H3Lib-Extensions-H3IndexExtensions-ToParent-H3Lib-H3Index,System-Int32-'></a>
### ToParent(h,parentRes) `method`

##### Summary

h3ToParent produces the parent index for a given H3 index

##### Returns

H3Index of the parent, or H3_NULL if you actually asked for a child

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | H3Index to find parent of |
| parentRes | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The resolution to switch to (parent, grandparent, etc) |

<a name='M-H3Lib-Extensions-H3IndexExtensions-Uncompact-H3Lib-H3Index,System-Int32-'></a>
### Uncompact(singleCell,res) `method`

##### Summary

Run uncompact on a single cell

##### Returns

[Uncompact](#M-H3Lib-Extensions-CollectionExtensions-Uncompact-System-Collections-Generic-List{H3Lib-H3Index},System-Int32- 'H3Lib.Extensions.CollectionExtensions.Uncompact(System.Collections.Generic.List{H3Lib.H3Index},System.Int32)') for details

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| singleCell | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | cell to uncompact |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | resolution to uncompact to |

<a name='M-H3Lib-Extensions-H3IndexExtensions-UniDirectionalEdgeTo-H3Lib-H3Index,H3Lib-H3Index-'></a>
### UniDirectionalEdgeTo(origin,destination) `method`

##### Summary

Returns a unidirectional edge H3 index based on the provided origin and destination

##### Returns

The unidirectional edge H3Index, or H3_NULL on failure.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The origin H3 hexagon index |
| destination | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The destination H3 hexagon index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-UniEdgeToGeoBoundary-H3Lib-H3Index-'></a>
### UniEdgeToGeoBoundary(edge) `method`

##### Summary

Provides the coordinates defining the unidirectional edge.

##### Returns

The geoboundary object to store the edge coordinates.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| edge | [H3Lib.H3Index](#T-H3Lib-H3Index 'H3Lib.H3Index') | The unidirectional edge H3Index |

<a name='M-H3Lib-Extensions-H3IndexExtensions-VertexNumForDirection-H3Lib-H3Index,H3Lib-Direction-'></a>
### VertexNumForDirection() `method`

##### Summary

Get the first vertex number for a given direction. The neighbor in this
direction is located between this vertex number and the next number in
sequence.

##### Returns

The number for the first topological vertex, or INVALID_VERTEX_NUM
if the direction is not valid for this cell

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3IndexExtensions-VertexRotations-H3Lib-H3Index-'></a>
### VertexRotations() `method`

##### Summary

Get the number of CCW rotations of the cell's vertex numbers
compared to the directional layout of its neighbors.

##### Returns

Number of CCW rotations for the cell

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-H3LibExtensions'></a>
## H3LibExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Extension methods that work on numbers that are then converted to some
parameter of H3Index space

<a name='M-H3Lib-Extensions-H3LibExtensions-Compact-System-Collections-Generic-List{H3Lib-H3Index}-'></a>
### Compact(h3Set) `method`

##### Summary

compact takes a set of hexagons all at the same resolution and compresses
them by pruning full child branches to the parent level. This is also done
for all parents recursively to get the minimum number of hex addresses that
perfectly cover the defined space.

##### Returns

status code and compacted hexes

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| h3Set | [System.Collections.Generic.List{H3Lib.H3Index}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.H3Index}') | Set of hexagons |

##### Remarks

Gonna do this a bit differently, allowing for varying
resolutions in input data.  Also, this is a front for [FlexiCompact](#M-H3Lib-Extensions-H3LibExtensions-FlexiCompact-System-Collections-Generic-List{H3Lib-H3Index}- 'H3Lib.Extensions.H3LibExtensions.FlexiCompact(System.Collections.Generic.List{H3Lib.H3Index})')
that tries to maintain the same restrictions the original H3 compact enforces.

<a name='M-H3Lib-Extensions-H3LibExtensions-ConstrainLatitude-System-Double-'></a>
### ConstrainLatitude(latitude) `method`

##### Summary

Makes sure latitudes are in the proper bounds

##### Returns

The corrected lat value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| latitude | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The original lat value |

<a name='M-H3Lib-Extensions-H3LibExtensions-ConstrainLatitude-System-Int32-'></a>
### ConstrainLatitude() `method`

##### Summary

Constrain Latitude to +/- PI/2

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3LibExtensions-ConstrainLongitude-System-Double-'></a>
### ConstrainLongitude(longitude) `method`

##### Summary

Makes sure longitudes are in the proper bounds

##### Returns

The corrected lng value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| longitude | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The origin lng value |

<a name='M-H3Lib-Extensions-H3LibExtensions-ConstrainLongitude-System-Int32-'></a>
### ConstrainLongitude(longitude) `method`

##### Summary

Constrain Longitude to +/- PI

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| longitude | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-H3Lib-Extensions-H3LibExtensions-DegreesToRadians-System-Double-'></a>
### DegreesToRadians(degrees) `method`

##### Summary

Convert from decimal degrees to radians.

##### Returns

The corresponding radians

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| degrees | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The decimal degrees |

<a name='M-H3Lib-Extensions-H3LibExtensions-DegreesToRadians-System-Int32-'></a>
### DegreesToRadians(degrees) `method`

##### Summary

Convert decimal degrees to radians

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| degrees | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-H3Lib-Extensions-H3LibExtensions-FlexiCompact-System-Collections-Generic-List{H3Lib-H3Index}-'></a>
### FlexiCompact() `method`

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3LibExtensions-GetPentagonIndexes-System-Int32-'></a>
### GetPentagonIndexes(res) `method`

##### Returns

Output List.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The resolution to produce pentagons at. |

<a name='M-H3Lib-Extensions-H3LibExtensions-IsResClassIii-System-Int32-'></a>
### IsResClassIii(res) `method`

##### Summary

Returns whether or not a resolution is a Class III grid. Note that odd
 resolutions are Class III and even resolutions are Class II.

##### Returns

Returns true if the resolution is class III grid, otherwise false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution |

<a name='M-H3Lib-Extensions-H3LibExtensions-IsValidChildRes-System-Int32,System-Int32-'></a>
### IsValidChildRes(parentRes,childRes) `method`

##### Summary

Determines whether one resolution is a valid child resolution of another.
Each resolution is considered a valid child resolution of itself.

##### Returns

The validity of the child resolution

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| parentRes | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | int resolution of the parent |
| childRes | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | int resolution of the child |

<a name='M-H3Lib-Extensions-H3LibExtensions-MaxKringSize-System-Int32-'></a>
### MaxKringSize(k) `method`

##### Summary

Maximum number of cells that result from the kRing algorithm with the given
k. Formula source and proof: https://oeis.org/A003215

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| k | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | k value, k >= 0. |

<a name='M-H3Lib-Extensions-H3LibExtensions-NormalizeLongitude-System-Double,System-Boolean-'></a>
### NormalizeLongitude() `method`

##### Summary

Normalize longitude, dealing with transmeridian arcs

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Extensions-H3LibExtensions-NormalizeRadians-System-Double,System-Double-'></a>
### NormalizeRadians(rads,limit) `method`

##### Summary

Normalizes radians to a value between 0.0 and two PI.

##### Returns

The normalized radians value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| rads | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The input radians value |
| limit | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | Default value of 2pi. _Can_ be changed, probably shouldn't |

##### Remarks

Originally part of geoCoord.c as  double _posAngleRads(double rads)

However, it's only used once in
void _geoAzDistanceRads(const GeoCoord *p1, double az, double distance, GeoCoord *p2)

It's used multiple times in faceijk.c, _geoToHex2d and _hex2dToGeo

For now, let's isolate it and see if it needs to be folded in later.

<a name='M-H3Lib-Extensions-H3LibExtensions-NumHexagons-System-Int32-'></a>
### NumHexagons(res) `method`

##### Summary

Number of unique valid H3Indexes at given resolution.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Resolution to get count of cells |

<a name='M-H3Lib-Extensions-H3LibExtensions-Power-System-Int64,System-Int64-'></a>
### Power(baseValue,power) `method`

##### Summary

Does integer exponentiation efficiently. Taken from StackOverflow.

 An example of this can be found at:
 https://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int

##### Returns

the exponentiated value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| baseValue | [System.Int64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int64 'System.Int64') | the integer base (can be positive or negative) |
| power | [System.Int64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int64 'System.Int64') | the integer exponent (should be nonnegative) |

<a name='M-H3Lib-Extensions-H3LibExtensions-RadiansToDegrees-System-Double-'></a>
### RadiansToDegrees(radians) `method`

##### Summary

Convert from radians to decimal degrees.

##### Returns

The corresponding decimal degrees

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| radians | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The radians |

<a name='M-H3Lib-Extensions-H3LibExtensions-Square-System-Double-'></a>
### Square(x) `method`

##### Summary

Square of a number

##### Returns

The square of the input number

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| x | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') | The input number |

<a name='M-H3Lib-Extensions-H3LibExtensions-ToH3Index-System-String-'></a>
### ToH3Index(s) `method`

##### Summary

Converts a string representation of an H3 index into an H3 index.

##### Returns

The H3 index corresponding to the string argument, or 0 if invalid.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| s | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The string representation of an H3 index. |

<a name='T-H3Lib-H3Mode'></a>
## H3Mode `type`

##### Namespace

H3Lib

##### Summary

mode for any examined H3Index

<a name='F-H3Lib-H3Mode-Hexagon'></a>
### Hexagon `constants`

##### Summary

Hexagon mode

<a name='F-H3Lib-H3Mode-UniEdge'></a>
### UniEdge `constants`

##### Summary

Directed Edge mode

<a name='T-H3Lib-LinkedGeoCoord'></a>
## LinkedGeoCoord `type`

##### Namespace

H3Lib

##### Summary

A wrapper class for storing GeoCoords within a linked list

<a name='M-H3Lib-LinkedGeoCoord-#ctor'></a>
### #ctor() `constructor`

##### Summary

constructor

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-LinkedGeoCoord-#ctor-H3Lib-GeoCoord-'></a>
### #ctor() `constructor`

##### Summary

constructor with vertex

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-LinkedGeoCoord-_gc'></a>
### _gc `constants`

##### Summary

Vertex being held

<a name='P-H3Lib-LinkedGeoCoord-Latitude'></a>
### Latitude `property`

##### Summary

Latitude of vertex

<a name='P-H3Lib-LinkedGeoCoord-Longitude'></a>
### Longitude `property`

##### Summary

longitude of vertex

<a name='P-H3Lib-LinkedGeoCoord-Vertex'></a>
### Vertex `property`

##### Summary

Return the actual vertex, read only

<a name='M-H3Lib-LinkedGeoCoord-Replacement-H3Lib-GeoCoord-'></a>
### Replacement() `method`

##### Summary

mutator to change vertex

##### Parameters

This method has no parameters.

<a name='T-H3Lib-LinkedGeoLoop'></a>
## LinkedGeoLoop `type`

##### Namespace

H3Lib

##### Summary

A loop node in a linked geo structure, part of a linked list

<a name='M-H3Lib-LinkedGeoLoop-#ctor'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-LinkedGeoLoop-Loop'></a>
### Loop `constants`

##### Summary

Linked list that stores the vertices

<a name='P-H3Lib-LinkedGeoLoop-Count'></a>
### Count `property`

##### Summary

Counts how many vetices in this loop

<a name='P-H3Lib-LinkedGeoLoop-First'></a>
### First `property`

##### Summary

Gets the first vertex in the list

<a name='P-H3Lib-LinkedGeoLoop-IsEmpty'></a>
### IsEmpty `property`

##### Summary

Indicates if there's any vertices in the loop

<a name='P-H3Lib-LinkedGeoLoop-Nodes'></a>
### Nodes `property`

##### Summary

Presents a copy of the vertices in a linear list

<a name='M-H3Lib-LinkedGeoLoop-AddLinkedCoord-H3Lib-GeoCoord-'></a>
### AddLinkedCoord(vertex) `method`

##### Summary

Add a new linked coordinate to the current loop

##### Returns

Reference to the coordinate

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vertex | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | Coordinate to add |

<a name='M-H3Lib-LinkedGeoLoop-Clear'></a>
### Clear() `method`

##### Summary

Clears the list of coords

##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoLoop-CopyNodes'></a>
### CopyNodes() `method`

##### Summary

Makes a copy of the vertices in the loop

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoLoop-Destroy'></a>
### Destroy() `method`

##### Summary

Clears the list of coords

##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoLoop-GetFirst'></a>
### GetFirst() `method`

##### Summary

Returns first vertex or null if there are none.

##### Returns



##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-LinkedGeoLoopExtensions'></a>
## LinkedGeoLoopExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations on LinkedGeoLoop type

<a name='M-H3Lib-Extensions-LinkedGeoLoopExtensions-CountContainers-H3Lib-LinkedGeoLoop,System-Collections-Generic-List{H3Lib-LinkedGeoPolygon},System-Collections-Generic-List{H3Lib-BBox}-'></a>
### CountContainers(loop,polygons,boxes) `method`

##### Summary

Count the number of polygons containing a given loop.

##### Returns

Number of polygons containing the loop

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') | Loop to count containers for |
| polygons | [System.Collections.Generic.List{H3Lib.LinkedGeoPolygon}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.LinkedGeoPolygon}') | Polygons to test |
| boxes | [System.Collections.Generic.List{H3Lib.BBox}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.BBox}') | Bounding boxes for polygons, used in point-in-poly check |

<a name='M-H3Lib-Extensions-LinkedGeoLoopExtensions-IsClockwise-H3Lib-LinkedGeoLoop-'></a>
### IsClockwise(loop) `method`

##### Summary

Is loop clockwise?

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') |  |

<a name='M-H3Lib-Extensions-LinkedGeoLoopExtensions-IsClockwiseNormalized-H3Lib-LinkedGeoLoop,System-Boolean-'></a>
### IsClockwiseNormalized(loop,isTransmeridian) `method`

##### Summary

Is loop clockwise normalized?

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') |  |
| isTransmeridian | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-H3Lib-Extensions-LinkedGeoLoopExtensions-PointInside-H3Lib-LinkedGeoLoop,H3Lib-BBox,H3Lib-GeoCoord-'></a>
### PointInside(loop,box,coord) `method`

##### Summary

Is point inside GeoLoop?

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') |  |
| box | [H3Lib.BBox](#T-H3Lib-BBox 'H3Lib.BBox') |  |
| coord | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |

<a name='M-H3Lib-Extensions-LinkedGeoLoopExtensions-ToBBox-H3Lib-LinkedGeoLoop-'></a>
### ToBBox(loop) `method`

##### Summary

Convert GeoLoop to bounding box for loop

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') |  |

<a name='T-H3Lib-LinkedGeoPolygon'></a>
## LinkedGeoPolygon `type`

##### Namespace

H3Lib

##### Summary

A polygon node in a linked geo structure, part of a linked list.

<a name='M-H3Lib-LinkedGeoPolygon-#ctor'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-LinkedGeoPolygon-Next'></a>
### Next `constants`

##### Summary

Returns reference to next polygon

<a name='F-H3Lib-LinkedGeoPolygon-_geoLoops'></a>
### _geoLoops `constants`

##### Summary

Linked list of loops that make up the polygon

<a name='P-H3Lib-LinkedGeoPolygon-CountLoops'></a>
### CountLoops `property`

##### Summary

Count of loops in polygon

<a name='P-H3Lib-LinkedGeoPolygon-CountPolygons'></a>
### CountPolygons `property`

##### Summary

Gets the count of polygons associated

<a name='P-H3Lib-LinkedGeoPolygon-First'></a>
### First `property`

##### Summary

Returns reference to the first loop

<a name='P-H3Lib-LinkedGeoPolygon-Last'></a>
### Last `property`

##### Summary

Returns reference to the last loop

<a name='P-H3Lib-LinkedGeoPolygon-LinkedPolygons'></a>
### LinkedPolygons `property`

##### Summary

Returns all linked polygons to this one as a linear list

<a name='P-H3Lib-LinkedGeoPolygon-Loops'></a>
### Loops `property`

##### Summary

Linear list of all loops in this specific polygon

<a name='M-H3Lib-LinkedGeoPolygon-AddLinkedLoop-H3Lib-LinkedGeoLoop-'></a>
### AddLinkedLoop(loop) `method`

##### Summary

Add an existing linked loop to the current polygon

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') | Reference to loop |

<a name='M-H3Lib-LinkedGeoPolygon-AddNewLinkedGeoPolygon'></a>
### AddNewLinkedGeoPolygon() `method`

##### Summary

Add a newly constructed polygon to current polygon.

##### Returns

Reference to new polygon

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='M-H3Lib-LinkedGeoPolygon-AddNewLinkedLoop'></a>
### AddNewLinkedLoop() `method`

##### Summary

Add a new linked loop to the current polygon

##### Returns

Reference to loop

##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoPolygon-Clear'></a>
### Clear() `method`

##### Summary

Free all the geoloops and propagate to the next polygon until
there's no more polygons.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoPolygon-Destroy'></a>
### Destroy() `method`

##### Summary

[Clear](#M-H3Lib-LinkedGeoPolygon-Clear 'H3Lib.LinkedGeoPolygon.Clear')

##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoPolygon-GetFirst'></a>
### GetFirst() `method`

##### Summary

Returns first loop if any exist, null otherwise

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoPolygon-GetLast'></a>
### GetLast() `method`

##### Summary

Gets last loop in polygon, null if none exist.

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoPolygon-GetPolygons'></a>
### GetPolygons() `method`

##### Summary

This is potentially dangerous, thus why it's
a private method and provided as read only.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-LinkedGeoPolygon-TotalPolygons'></a>
### TotalPolygons() `method`

##### Summary

Count the number of polygons in a linked list

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-LinkedGeoPolygonExtensions'></a>
## LinkedGeoPolygonExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations on LinkedGeoPolygon type

<a name='M-H3Lib-Extensions-LinkedGeoPolygonExtensions-FindPolygonForHole-H3Lib-LinkedGeoLoop,H3Lib-LinkedGeoPolygon,System-Collections-Generic-List{H3Lib-BBox},System-Int32-'></a>
### FindPolygonForHole(loop,polygon,boxes,polygonCount) `method`

##### Summary

Find the polygon to which a given hole should be allocated. Note that this
function will return null if no parent is found.

##### Returns

Pointer to parent polygon, or null if not found

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loop | [H3Lib.LinkedGeoLoop](#T-H3Lib-LinkedGeoLoop 'H3Lib.LinkedGeoLoop') | Inner loop describing a hole |
| polygon | [H3Lib.LinkedGeoPolygon](#T-H3Lib-LinkedGeoPolygon 'H3Lib.LinkedGeoPolygon') | Head of a linked list of polygons to check |
| boxes | [System.Collections.Generic.List{H3Lib.BBox}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{H3Lib.BBox}') | Bounding boxes for polygons, used in point-in-poly check |
| polygonCount | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of polygons to check |

<a name='M-H3Lib-Extensions-LinkedGeoPolygonExtensions-NormalizeMultiPolygon-H3Lib-LinkedGeoPolygon-'></a>
### NormalizeMultiPolygon(root) `method`

##### Summary

Normalize a LinkedGeoPolygon in-place into a structure following GeoJSON
 MultiPolygon rules: Each polygon must have exactly one outer loop, which
 must be first in the list, followed by any holes. Holes in this algorithm
 are identified by winding order (holes are clockwise), which is guaranteed
 by the h3SetToVertexGraph algorithm.

 Input to this function is assumed to be a single polygon including all
 loops to normalize. It's assumed that a valid arrangement is possible.

##### Returns

Tuple
 Item1 - 0 on success, or an error code > 0 for invalid input
 Item2 - Normalized LinkedGeoPolygon

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| root | [H3Lib.LinkedGeoPolygon](#T-H3Lib-LinkedGeoPolygon 'H3Lib.LinkedGeoPolygon') | Root polygon including all loops |

<a name='T-H3Lib-Constants-LocalIJ'></a>
## LocalIJ `type`

##### Namespace

H3Lib.Constants

<a name='F-H3Lib-Constants-LocalIJ-FAILED_DIRECTIONS'></a>
### FAILED_DIRECTIONS `constants`

<a name='F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS'></a>
### PENTAGON_ROTATIONS `constants`

##### Summary

Origin leading digit -> index leading digit -> rotations 60 cw
Either being 1 (K axis) is invalid.
No good default at 0.

<a name='F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS_REVERSE'></a>
### PENTAGON_ROTATIONS_REVERSE `constants`

##### Summary

Reverse base cell direction -> leading index digit -> rotations 60 ccw.
For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
on a pentagon and the origin is not.

<a name='F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS_REVERSE_NONPOLAR'></a>
### PENTAGON_ROTATIONS_REVERSE_NONPOLAR `constants`

##### Summary

Reverse base cell direction -> leading index digit -> rotations 60 ccw.
For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
on a pentagon and the origin is not.

<a name='F-H3Lib-Constants-LocalIJ-PENTAGON_ROTATIONS_REVERSE_POLAR'></a>
### PENTAGON_ROTATIONS_REVERSE_POLAR `constants`

##### Summary

Reverse base cell direction -> leading index digit -> rotations 60 ccw.
For reversing the rotation introduced in PENTAGON_ROTATIONS when the index is
on a polar pentagon and the origin is not.

<a name='T-H3Lib-Overage'></a>
## Overage `type`

##### Namespace

H3Lib

##### Summary

Digit representing overage type

<a name='F-H3Lib-Overage-FACE_EDGE'></a>
### FACE_EDGE `constants`

##### Summary

Overage at face edge

<a name='F-H3Lib-Overage-NEW_FACE'></a>
### NEW_FACE `constants`

##### Summary

Overage goes on next face

<a name='F-H3Lib-Overage-NO_OVERAGE'></a>
### NO_OVERAGE `constants`

##### Summary

No overage

<a name='T-H3Lib-PentagonDirectionFace'></a>
## PentagonDirectionFace `type`

##### Namespace

H3Lib

##### Summary

The faces in each axial direction of a given pentagon base cell

<a name='M-H3Lib-PentagonDirectionFace-#ctor-System-Int32,System-Collections-Generic-IList{System-Int32}-'></a>
### #ctor(bc,faces) `constructor`

##### Summary

Constructor

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| bc | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| faces | [System.Collections.Generic.IList{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IList 'System.Collections.Generic.IList{System.Int32}') |  |

<a name='M-H3Lib-PentagonDirectionFace-#ctor-System-Collections-Generic-IList{System-Int32}-'></a>
### #ctor(raw) `constructor`

##### Summary

Constructor

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| raw | [System.Collections.Generic.IList{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IList 'System.Collections.Generic.IList{System.Int32}') |  |

<a name='M-H3Lib-PentagonDirectionFace-#ctor-System-Int32,System-Int32,System-Int32,System-Int32,System-Int32,System-Int32-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-PentagonDirectionFace-BaseCell'></a>
### BaseCell `constants`

##### Summary

base cell number

<a name='F-H3Lib-PentagonDirectionFace-Faces'></a>
### Faces `constants`

##### Summary

face numbers for each axial direction, in order, starting with J

<a name='T-H3Lib-Vec2d'></a>
## Vec2d `type`

##### Namespace

H3Lib

##### Summary

2D floating point vector functions.

<a name='M-H3Lib-Vec2d-#ctor-System-Double,System-Double-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-Vec2d-X'></a>
### X `constants`

##### Summary

X coordinate

<a name='F-H3Lib-Vec2d-Y'></a>
### Y `constants`

##### Summary

Y Coordinate

<a name='P-H3Lib-Vec2d-Magnitude'></a>
### Magnitude `property`

##### Summary

Calculates the magnitude of a 2D cartesian vector.

<a name='M-H3Lib-Vec2d-Equals-H3Lib-Vec2d-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec2d-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality test against unboxed object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec2d-FindIntersection-H3Lib-Vec2d,H3Lib-Vec2d,H3Lib-Vec2d,H3Lib-Vec2d-'></a>
### FindIntersection(p0,p1,p2,p3) `method`

##### Summary

Finds the intersection between two lines. Assumes that the lines intersect
and that the intersection is not at an endpoint of either line.

##### Returns

The intersection point.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| p0 | [H3Lib.Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d') | The first endpoint of the first line |
| p1 | [H3Lib.Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d') | The second endpoint of the first line |
| p2 | [H3Lib.Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d') | The first endpoint of the second line |
| p3 | [H3Lib.Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d') | The first endpoint of the first line |

<a name='M-H3Lib-Vec2d-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec2d-ToString'></a>
### ToString() `method`

##### Summary

Debug info as string

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec2d-op_Equality-H3Lib-Vec2d,H3Lib-Vec2d-'></a>
### op_Equality() `method`

##### Summary

Equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec2d-op_Inequality-H3Lib-Vec2d,H3Lib-Vec2d-'></a>
### op_Inequality() `method`

##### Summary

Inequality operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-Vec2dExtensions'></a>
## Vec2dExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations on Vec2d

<a name='M-H3Lib-Extensions-Vec2dExtensions-ToCoordIjk-H3Lib-Vec2d-'></a>
### ToCoordIjk(v) `method`

##### Summary

Determine the containing hex in ijk+ coordinates for a 2D cartesian
coordinate vector (from ).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| v | [H3Lib.Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d') | The 2D cartesian coordinate vector. |

<a name='M-H3Lib-Extensions-Vec2dExtensions-ToGeoCoord-H3Lib-Vec2d,System-Int32,System-Int32,System-Int32-'></a>
### ToGeoCoord(v,face,res,substrate) `method`

##### Summary

Determines the center point in spherical coordinates of a cell given by 2D
hex coordinates on a particular icosahedral face.

##### Returns

The spherical coordinates of the cell center point

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| v | [H3Lib.Vec2d](#T-H3Lib-Vec2d 'H3Lib.Vec2d') | The 2D hex coordinates of the cell |
| face | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The icosahedral face upon which the 2D hex coordinate system is centered |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The H3 resolution of the cell |
| substrate | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Indicates whether or not this grid is actually a substrate
grid relative to the specified resolution. |

<a name='T-H3Lib-Vec3d'></a>
## Vec3d `type`

##### Namespace

H3Lib

##### Summary

3D floating point structure

<a name='M-H3Lib-Vec3d-#ctor-System-Double,System-Double,System-Double-'></a>
### #ctor() `constructor`

##### Summary

Constructor

##### Parameters

This constructor has no parameters.

<a name='F-H3Lib-Vec3d-X'></a>
### X `constants`

##### Summary

X Coordinate

<a name='F-H3Lib-Vec3d-Y'></a>
### Y `constants`

##### Summary

Y Coordinate

<a name='F-H3Lib-Vec3d-Z'></a>
### Z `constants`

##### Summary

Z Coordinate

<a name='M-H3Lib-Vec3d-Equals-H3Lib-Vec3d-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec3d-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec3d-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec3d-ToString'></a>
### ToString() `method`

##### Summary

Debug info in string format

##### Parameters

This method has no parameters.

<a name='M-H3Lib-Vec3d-op_Equality-H3Lib-Vec3d,H3Lib-Vec3d-'></a>
### op_Equality(left,right) `method`

##### Summary

Equality operator

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| left | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') |  |
| right | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') |  |

<a name='M-H3Lib-Vec3d-op_Inequality-H3Lib-Vec3d,H3Lib-Vec3d-'></a>
### op_Inequality() `method`

##### Summary

inequality operator

##### Parameters

This method has no parameters.

<a name='T-H3Lib-Extensions-Vec3dExtensions'></a>
## Vec3dExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations on Vec3d

<a name='M-H3Lib-Extensions-Vec3dExtensions-PointSquareDistance-H3Lib-Vec3d,H3Lib-Vec3d-'></a>
### PointSquareDistance(v1,v2) `method`

##### Summary

Calculate the square of the distance between two 3D coordinates.

##### Returns

The square of the distance between the given points.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| v1 | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') | The first 3D coordinate. |
| v2 | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') | The second 3D coordinate. |

<a name='M-H3Lib-Extensions-Vec3dExtensions-SetX-H3Lib-Vec3d,System-Double-'></a>
### SetX(v3,x) `method`

##### Summary

Replace X value

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| v3 | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') |  |
| x | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') |  |

<a name='M-H3Lib-Extensions-Vec3dExtensions-SetY-H3Lib-Vec3d,System-Double-'></a>
### SetY(v3,y) `method`

##### Summary

Replace Y value

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| v3 | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') |  |
| y | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') |  |

<a name='M-H3Lib-Extensions-Vec3dExtensions-SetZ-H3Lib-Vec3d,System-Double-'></a>
### SetZ(v3,z) `method`

##### Summary

Repalce Z value

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| v3 | [H3Lib.Vec3d](#T-H3Lib-Vec3d 'H3Lib.Vec3d') |  |
| z | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') |  |

<a name='T-H3Lib-Constants-Vertex'></a>
## Vertex `type`

##### Namespace

H3Lib.Constants

<a name='F-H3Lib-Constants-Vertex-DirectionToVertexNumHex'></a>
### DirectionToVertexNumHex `constants`

##### Summary

Hexagon direction to vertex number relationships (same face).

 Note that we don't use direction 0 (center).

<a name='F-H3Lib-Constants-Vertex-DirectionToVertexNumPent'></a>
### DirectionToVertexNumPent `constants`

##### Summary

Pentagon direction to vertex number relationships (same face).
Note that we don't use directions 0 (center) or 1 (deleted K axis).

<a name='F-H3Lib-Constants-Vertex-INVALID_VERTEX_NUM'></a>
### INVALID_VERTEX_NUM `constants`

##### Summary

Invalid vertex number

<a name='F-H3Lib-Constants-Vertex-MAX_BASE_CELL_FACES'></a>
### MAX_BASE_CELL_FACES `constants`

##### Summary

Max number of faces a base cell's descendants may appear on

<a name='F-H3Lib-Constants-Vertex-PentagonDirectionFaces'></a>
### PentagonDirectionFaces `constants`

##### Summary

Table of direction-to-face mapping for each pentagon

 Note that faces are in directional order, starting at J_AXES_DIGIT.
 This table is generated by the generatePentagonDirectionFaces script.

##### Remarks

TODO: Need to create generatePentagonDirectionFaces script equivalent

<a name='T-H3Lib-VertexGraph'></a>
## VertexGraph `type`

##### Namespace

H3Lib

##### Summary

Data structure for storing a graph of vertices

<a name='M-H3Lib-VertexGraph-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initialize a new VertexGraph

##### Parameters

This constructor has no parameters.

<a name='M-H3Lib-VertexGraph-#ctor-System-Int32-'></a>
### #ctor(res) `constructor`

##### Summary

Initialize a new VertexGraph

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| res | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Resolution of the hexagons whose vertices we're storing |

<a name='F-H3Lib-VertexGraph-Resolution'></a>
### Resolution `constants`

##### Summary

Resolution of nodes, but not needed. Probably needs to be phased out

<a name='F-H3Lib-VertexGraph-_pool'></a>
### _pool `constants`

##### Summary

Hashset to store vertices

<a name='P-H3Lib-VertexGraph-Count'></a>
### Count `property`

##### Summary

Number of vertices

<a name='P-H3Lib-VertexGraph-Size'></a>
### Size `property`

##### Summary

Number of vertices

<a name='M-H3Lib-VertexGraph-AddNode-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### AddNode(fromNode,toNode) `method`

##### Summary

Add an edge to the graph

##### Returns

Reference to the new node

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fromNode | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | Start vertex |
| toNode | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | End vertex |

##### Remarks

Gonna try some tomfoolery here, and if you add
a node that already exists (in either direction)
then remove it in both directions.

<a name='M-H3Lib-VertexGraph-Clear'></a>
### Clear() `method`

##### Summary

Destroy a VertexGraph's sub-objects, freeing their memory. The caller is
responsible for freeing memory allocated to the VertexGraph struct itself.

##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexGraph-FindEdge-H3Lib-GeoCoord,System-Nullable{H3Lib-GeoCoord}-'></a>
### FindEdge(fromNode,toNode) `method`

##### Summary

Find the Vertex node for a given edge, if it exists

##### Returns

Reference to the vertex node, if found

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fromNode | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | Start vertex |
| toNode | [System.Nullable{H3Lib.GeoCoord}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{H3Lib.GeoCoord}') | End vertex, or NULL if we don't care |

<a name='M-H3Lib-VertexGraph-FindVertex-H3Lib-GeoCoord-'></a>
### FindVertex(vertex) `method`

##### Summary

Find a Vertex node starting at the given vertex

##### Returns

Pointer to the vertex node, if found

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vertex | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') | fromVtx Start vertex |

<a name='M-H3Lib-VertexGraph-FirstNode'></a>
### FirstNode() `method`

##### Summary

Picks whatever HashSet says is the first VertexNode

##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexGraph-InitNode-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### InitNode() `method`

##### Summary

Create a new node based on two GeoCoords

##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexGraph-RemoveNode-System-Nullable{H3Lib-VertexNode}-'></a>
### RemoveNode(vn) `method`

##### Summary

Remove a node from the graph. The input node will be freed, and should
not be used after removal.

##### Returns

true on success, false on faiilure (node not found)

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vn | [System.Nullable{H3Lib.VertexNode}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{H3Lib.VertexNode}') | Node to remove |

<a name='T-H3Lib-Extensions-VertexGraphExtensions'></a>
## VertexGraphExtensions `type`

##### Namespace

H3Lib.Extensions

##### Summary

Operations on VertexGraph type

<a name='M-H3Lib-Extensions-VertexGraphExtensions-ToLinkedGeoPolygon-H3Lib-VertexGraph-'></a>
### ToLinkedGeoPolygon(graph) `method`

##### Summary

Internal: Create a LinkedGeoPolygon from a vertex graph. It is the
responsibility of the caller to call destroyLinkedPolygon on the populated
linked geo structure, or the memory for that structure will not be freed.

##### Returns

Output polygon

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| graph | [H3Lib.VertexGraph](#T-H3Lib-VertexGraph 'H3Lib.VertexGraph') | Input graph |

<a name='T-H3Lib-VertexNode'></a>
## VertexNode `type`

##### Namespace

H3Lib

##### Summary

A single node in a vertex graph, part of a linked list

<a name='M-H3Lib-VertexNode-#ctor-H3Lib-GeoCoord,H3Lib-GeoCoord-'></a>
### #ctor(toNode,fromNode) `constructor`

##### Summary

Constructor

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| toNode | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |
| fromNode | [H3Lib.GeoCoord](#T-H3Lib-GeoCoord 'H3Lib.GeoCoord') |  |

<a name='F-H3Lib-VertexNode-From'></a>
### From `constants`

##### Summary

Where the edge starts

<a name='F-H3Lib-VertexNode-To'></a>
### To `constants`

##### Summary

Where the edge ends

<a name='M-H3Lib-VertexNode-Equals-H3Lib-VertexNode-'></a>
### Equals() `method`

##### Summary

Equality test

##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexNode-Equals-System-Object-'></a>
### Equals() `method`

##### Summary

Equality test against unboxed object

##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexNode-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Hashcode for identity

##### Returns



##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexNode-op_Equality-H3Lib-VertexNode,H3Lib-VertexNode-'></a>
### op_Equality() `method`

##### Summary

equality operator

##### Parameters

This method has no parameters.

<a name='M-H3Lib-VertexNode-op_Inequality-H3Lib-VertexNode,H3Lib-VertexNode-'></a>
### op_Inequality() `method`

##### Summary

inequality operator

##### Parameters

This method has no parameters.
