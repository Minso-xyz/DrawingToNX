using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NXOpen;
using NXOpen.CAE;
using NXOpen.Features;
using NXOpen.UF;

namespace DrawingToNXBuilder
{
    public class NXBuilder
    {
        public static void Main(string[] args)
        {
            Session session = Session.GetSession();
            Part workPart = session.Parts.Work;

            ListingWindow lw = session.ListingWindow;
            lw.Open();
            lw.WriteLine("DrawingToNXBuilder started.");

            // create the plate
            Body plateBody = CreatePlate(session, workPart);
            lw.WriteLine("The plate 50 x 40 x 3 [mm] has been generated.");

            // Create the holes
            PointFeature[] holePoints = CreateHolePoints(workPart, new Point3d[]
        {
                new Point3d(10.0, 10.0, 3.0),
                new Point3d(40.0, 10.0, 3.0),
                new Point3d(10.0, 30.0, 3.0),
                new Point3d(40.0, 30.0, 3.0)
        });
            lw.WriteLine("The points have been generated.");

            CreateThroughHoles(session, workPart, plateBody, holePoints, 5.5);
            lw.WriteLine("The holes have been generated.");
        }

        private static Body CreatePlate(Session theSession, Part workPart)
        {
            NXOpen.Part displayPart = theSession.Parts.Display;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Sketch");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Model from Sketch");

            theSession.BeginTaskEnvironment();

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Sketch nullNXOpen_Sketch = null;
            NXOpen.SketchInPlaceBuilder sketchInPlaceBuilder1;
            sketchInPlaceBuilder1 = workPart.Sketches.CreateSketchInPlaceBuilder2(nullNXOpen_Sketch);

            NXOpen.Point3d origin1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Vector3d normal1 = new NXOpen.Vector3d(0.0, 0.0, 1.0);
            NXOpen.Plane plane1;
            plane1 = workPart.Planes.CreatePlane(origin1, normal1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            sketchInPlaceBuilder1.PlaneReference = plane1;

            NXOpen.Unit unit1 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
            NXOpen.Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.Expression expression2;
            expression2 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            NXOpen.SketchAlongPathBuilder sketchAlongPathBuilder1;
            sketchAlongPathBuilder1 = workPart.Sketches.CreateSketchAlongPathBuilder(nullNXOpen_Sketch);

            NXOpen.SimpleSketchInPlaceBuilder simpleSketchInPlaceBuilder1;
            simpleSketchInPlaceBuilder1 = workPart.Sketches.CreateSimpleSketchInPlaceBuilder();

            sketchAlongPathBuilder1.PlaneLocation.Expression.SetFormula("0");

            theSession.SetUndoMarkName(markId3, "Create Sketch Dialog");

            simpleSketchInPlaceBuilder1.UseWorkPartOrigin = false;

            NXOpen.Point3d coordinates1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Point point1;
            point1 = workPart.Points.CreatePoint(coordinates1);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(point1);

            NXOpen.Session.UndoMarkId markId4;
            markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Create Sketch");

            theSession.DeleteUndoMark(markId4, null);

            NXOpen.Session.UndoMarkId markId5;
            markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Create Sketch");

            NXOpen.Point3d origin2 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Vector3d vector1 = new NXOpen.Vector3d(1.0, 0.0, 0.0);
            NXOpen.Direction direction1;
            direction1 = workPart.Directions.CreateDirection(origin2, vector1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Point3d coordinates2 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Point point2;
            point2 = workPart.Points.CreatePoint(coordinates2);

            NXOpen.Point3d origin3 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Matrix3x3 matrix1 = new NXOpen.Matrix3x3();
            matrix1.Xx = 1.0;
            matrix1.Xy = 0.0;
            matrix1.Xz = 0.0;
            matrix1.Yx = 0.0;
            matrix1.Yy = 1.0;
            matrix1.Yz = 0.0;
            matrix1.Zx = 0.0;
            matrix1.Zy = 0.0;
            matrix1.Zz = 1.0;
            NXOpen.Plane plane2;
            plane2 = workPart.Planes.CreateFixedTypePlane(origin3, matrix1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            NXOpen.Xform xform1;
            xform1 = workPart.Xforms.CreateXformByPlaneXDirPoint(plane2, direction1, point2, NXOpen.SmartObject.UpdateOption.WithinModeling, 1.0, false, false);

            NXOpen.CartesianCoordinateSystem cartesianCoordinateSystem1;
            cartesianCoordinateSystem1 = workPart.CoordinateSystems.CreateCoordinateSystem(xform1, NXOpen.SmartObject.UpdateOption.WithinModeling);

            simpleSketchInPlaceBuilder1.CoordinateSystem = cartesianCoordinateSystem1;

            theSession.Preferences.Sketch.CreateInferredConstraints = false;

            theSession.Preferences.Sketch.ContinuousAutoDimensioning = false;

            theSession.Preferences.Sketch.DimensionLabel = NXOpen.Preferences.SketchPreferences.DimensionLabelType.Expression;

            theSession.Preferences.Sketch.TextSizeFixed = false;

            theSession.Preferences.Sketch.FixedTextSize = 3.0;

            theSession.Preferences.Sketch.DisplayParenthesesOnReferenceDimensions = true;

            theSession.Preferences.Sketch.DisplayReferenceGeometry = false;

            theSession.Preferences.Sketch.DisplayShadedRegions = true;

            theSession.Preferences.Sketch.FindMovableObjects = true;

            theSession.Preferences.Sketch.ConstraintSymbolSize = 3.0;

            theSession.Preferences.Sketch.DisplayObjectColor = false;

            theSession.Preferences.Sketch.DisplayObjectName = true;

            theSession.Preferences.Sketch.EditDimensionOnCreation = true;

            theSession.Preferences.Sketch.CreateDimensionForTypedValues = true;

            theSession.Preferences.Sketch.AddRemoteFindingAfterSnap = true;

            NXOpen.NXObject nXObject1;
            nXObject1 = simpleSketchInPlaceBuilder1.Commit();

            NXOpen.Sketch sketch1 = ((NXOpen.Sketch)nXObject1);
            NXOpen.Features.Feature feature1;
            feature1 = sketch1.Feature;

            NXOpen.Session.UndoMarkId markId6;
            markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "update");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId6);

            sketch1.Activate(NXOpen.Sketch.ViewReorient.True);

            theSession.Preferences.Sketch.FindMovableObjects = true;

            NXOpen.SketchFindMovableObjectsBuilder sketchFindMovableObjectsBuilder1;
            sketchFindMovableObjectsBuilder1 = workPart.Sketches.CreateFindMovableObjectsBuilder();

            NXOpen.NXObject nXObject2;
            nXObject2 = sketchFindMovableObjectsBuilder1.Commit();

            sketchFindMovableObjectsBuilder1.Destroy();

            theSession.DeleteUndoMark(markId5, null);

            theSession.SetUndoMarkName(markId3, "Create Sketch");

            sketchInPlaceBuilder1.Destroy();

            sketchAlongPathBuilder1.Destroy();

            simpleSketchInPlaceBuilder1.Destroy();

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression2);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            try
            {
                // Expression is still in use.
                workPart.Expressions.Delete(expression1);
            }
            catch (NXException ex)
            {
                ex.AssertErrorCode(1050029);
            }

            plane1.DestroyPlane();

            theSession.DeleteUndoMarksUpToMark(markId2, null, true);

            NXOpen.Session.UndoMarkId markId7;
            markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Open Sketch");

            theSession.ActiveSketch.SetName("SKETCH_000");

            // ----------------------------------------------
            //   Menu: Insert->Curve->Rectangle...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId8;
            markId8 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Profile short list");

            NXOpen.Session.UndoMarkId markId9;
            markId9 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Create Rectangle");

            NXOpen.Expression expression3;
            expression3 = workPart.Expressions.CreateSystemExpression("50");

            NXOpen.Expression expression4;
            expression4 = workPart.Expressions.CreateSystemExpression("40");

            workPart.Expressions.Edit(expression3, "50");

            theSession.SetUndoMarkVisibility(markId9, "Create Rectangle", NXOpen.Session.MarkVisibility.Visible);

            // ----------------------------------------------
            // Creating rectangle using By 2 Points method 
            // ----------------------------------------------
            NXOpen.Point3d startPoint1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Point3d endPoint1 = new NXOpen.Point3d(50.0, 0.0, 0.0);
            NXOpen.Line line1;
            line1 = workPart.Curves.CreateLine(startPoint1, endPoint1);

            NXOpen.Point3d startPoint2 = new NXOpen.Point3d(50.0, 0.0, 0.0);
            NXOpen.Point3d endPoint2 = new NXOpen.Point3d(50.0, 40.0, 0.0);
            NXOpen.Line line2;
            line2 = workPart.Curves.CreateLine(startPoint2, endPoint2);

            NXOpen.Point3d startPoint3 = new NXOpen.Point3d(50.0, 40.0, 0.0);
            NXOpen.Point3d endPoint3 = new NXOpen.Point3d(0.0, 40.0, 0.0);
            NXOpen.Line line3;
            line3 = workPart.Curves.CreateLine(startPoint3, endPoint3);

            NXOpen.Point3d startPoint4 = new NXOpen.Point3d(0.0, 40.0, 0.0);
            NXOpen.Point3d endPoint4 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            NXOpen.Line line4;
            line4 = workPart.Curves.CreateLine(startPoint4, endPoint4);

            theSession.ActiveSketch.AddGeometry(line1, NXOpen.Sketch.InferConstraintsOption.InferNoConstraints);

            theSession.ActiveSketch.AddGeometry(line2, NXOpen.Sketch.InferConstraintsOption.InferNoConstraints);

            theSession.ActiveSketch.AddGeometry(line3, NXOpen.Sketch.InferConstraintsOption.InferNoConstraints);

            theSession.ActiveSketch.AddGeometry(line4, NXOpen.Sketch.InferConstraintsOption.InferNoConstraints);

            NXOpen.Sketch.ConstraintGeometry conGeom1_1 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom1_1.Geometry = line1;
            conGeom1_1.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom1_1.SplineDefiningPointIndex = 0;
            NXOpen.Sketch.ConstraintGeometry conGeom2_1 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom2_1.Geometry = line2;
            conGeom2_1.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom2_1.SplineDefiningPointIndex = 0;
            NXOpen.SketchGeometricConstraint sketchGeometricConstraint1;
            sketchGeometricConstraint1 = theSession.ActiveSketch.CreatePerpendicularConstraint(conGeom1_1, conGeom2_1);

            NXOpen.Sketch.ConstraintGeometry conGeom1_2 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom1_2.Geometry = line2;
            conGeom1_2.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom1_2.SplineDefiningPointIndex = 0;
            NXOpen.Sketch.ConstraintGeometry conGeom2_2 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom2_2.Geometry = line3;
            conGeom2_2.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom2_2.SplineDefiningPointIndex = 0;
            NXOpen.SketchGeometricConstraint sketchGeometricConstraint2;
            sketchGeometricConstraint2 = theSession.ActiveSketch.CreatePerpendicularConstraint(conGeom1_2, conGeom2_2);

            NXOpen.Sketch.ConstraintGeometry conGeom1_3 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom1_3.Geometry = line3;
            conGeom1_3.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom1_3.SplineDefiningPointIndex = 0;
            NXOpen.Sketch.ConstraintGeometry conGeom2_3 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom2_3.Geometry = line4;
            conGeom2_3.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom2_3.SplineDefiningPointIndex = 0;
            NXOpen.SketchGeometricConstraint sketchGeometricConstraint3;
            sketchGeometricConstraint3 = theSession.ActiveSketch.CreatePerpendicularConstraint(conGeom1_3, conGeom2_3);

            NXOpen.Sketch.ConstraintGeometry conGeom1_4 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom1_4.Geometry = line4;
            conGeom1_4.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom1_4.SplineDefiningPointIndex = 0;
            NXOpen.Sketch.ConstraintGeometry conGeom2_4 = new NXOpen.Sketch.ConstraintGeometry();
            conGeom2_4.Geometry = line1;
            conGeom2_4.PointType = NXOpen.Sketch.ConstraintPointType.None;
            conGeom2_4.SplineDefiningPointIndex = 0;
            NXOpen.SketchGeometricConstraint sketchGeometricConstraint4;
            sketchGeometricConstraint4 = theSession.ActiveSketch.CreatePerpendicularConstraint(conGeom1_4, conGeom2_4);

            workPart.Expressions.Delete(expression3);

            workPart.Expressions.Delete(expression4);

            NXOpen.Sketch.DimensionGeometry dimObject1_1 = new NXOpen.Sketch.DimensionGeometry();
            dimObject1_1.Geometry = line1;
            dimObject1_1.AssocType = NXOpen.Sketch.AssocType.StartPoint;
            dimObject1_1.AssocValue = 0;
            dimObject1_1.HelpPoint.X = 0.0;
            dimObject1_1.HelpPoint.Y = 0.0;
            dimObject1_1.HelpPoint.Z = 0.0;
            NXOpen.NXObject nullNXOpen_NXObject = null;
            dimObject1_1.View = nullNXOpen_NXObject;
            NXOpen.Sketch.DimensionGeometry dimObject2_1 = new NXOpen.Sketch.DimensionGeometry();
            dimObject2_1.Geometry = line1;
            dimObject2_1.AssocType = NXOpen.Sketch.AssocType.EndPoint;
            dimObject2_1.AssocValue = 0;
            dimObject2_1.HelpPoint.X = 0.0;
            dimObject2_1.HelpPoint.Y = 0.0;
            dimObject2_1.HelpPoint.Z = 0.0;
            dimObject2_1.View = nullNXOpen_NXObject;
            NXOpen.Point3d dimOrigin1 = new NXOpen.Point3d(25.0, -44.402293910430878, 0.0);
            NXOpen.Expression nullNXOpen_Expression = null;
            NXOpen.SketchDimensionalConstraint sketchDimensionalConstraint1;
            sketchDimensionalConstraint1 = theSession.ActiveSketch.CreateDimension(NXOpen.Sketch.ConstraintType.ParallelDim, dimObject1_1, dimObject2_1, dimOrigin1, nullNXOpen_Expression, NXOpen.Sketch.DimensionOption.CreateAsConstant);

            NXOpen.SketchHelpedDimensionalConstraint sketchHelpedDimensionalConstraint1 = ((NXOpen.SketchHelpedDimensionalConstraint)sketchDimensionalConstraint1);
            NXOpen.Annotations.Dimension dimension1;
            dimension1 = sketchHelpedDimensionalConstraint1.AssociatedDimension;

            NXOpen.Expression expression5;
            expression5 = sketchHelpedDimensionalConstraint1.AssociatedExpression;

            NXOpen.Sketch.DimensionGeometry dimObject1_2 = new NXOpen.Sketch.DimensionGeometry();
            dimObject1_2.Geometry = line2;
            dimObject1_2.AssocType = NXOpen.Sketch.AssocType.StartPoint;
            dimObject1_2.AssocValue = 0;
            dimObject1_2.HelpPoint.X = 0.0;
            dimObject1_2.HelpPoint.Y = 0.0;
            dimObject1_2.HelpPoint.Z = 0.0;
            dimObject1_2.View = nullNXOpen_NXObject;
            NXOpen.Sketch.DimensionGeometry dimObject2_2 = new NXOpen.Sketch.DimensionGeometry();
            dimObject2_2.Geometry = line2;
            dimObject2_2.AssocType = NXOpen.Sketch.AssocType.EndPoint;
            dimObject2_2.AssocValue = 0;
            dimObject2_2.HelpPoint.X = 0.0;
            dimObject2_2.HelpPoint.Y = 0.0;
            dimObject2_2.HelpPoint.Z = 0.0;
            dimObject2_2.View = nullNXOpen_NXObject;
            NXOpen.Point3d dimOrigin2 = new NXOpen.Point3d(94.40229391043087, 20.0, 0.0);
            NXOpen.SketchDimensionalConstraint sketchDimensionalConstraint2;
            sketchDimensionalConstraint2 = theSession.ActiveSketch.CreateDimension(NXOpen.Sketch.ConstraintType.ParallelDim, dimObject1_2, dimObject2_2, dimOrigin2, nullNXOpen_Expression, NXOpen.Sketch.DimensionOption.CreateAsConstant);

            NXOpen.SketchHelpedDimensionalConstraint sketchHelpedDimensionalConstraint2 = ((NXOpen.SketchHelpedDimensionalConstraint)sketchDimensionalConstraint2);
            NXOpen.Annotations.Dimension dimension2;
            dimension2 = sketchHelpedDimensionalConstraint2.AssociatedDimension;

            NXOpen.Expression expression6;
            expression6 = sketchHelpedDimensionalConstraint2.AssociatedExpression;

            theSession.Preferences.Sketch.AutoDimensionsToArcCenter = false;

            theSession.ActiveSketch.Update();

            theSession.Preferences.Sketch.AutoDimensionsToArcCenter = true;

            NXOpen.SmartObject[] geoms1 = new NXOpen.SmartObject[4];
            geoms1[0] = line1;
            geoms1[1] = line2;
            geoms1[2] = line3;
            geoms1[3] = line4;
            theSession.ActiveSketch.UpdateConstraintDisplay(geoms1);

            NXOpen.SmartObject[] geoms2 = new NXOpen.SmartObject[4];
            geoms2[0] = line1;
            geoms2[1] = line2;
            geoms2[2] = line3;
            geoms2[3] = line4;
            theSession.ActiveSketch.UpdateDimensionDisplay(geoms2);

            NXOpen.NXObject[] objects1 = new NXOpen.NXObject[4];
            objects1[0] = sketchGeometricConstraint1;
            objects1[1] = sketchGeometricConstraint2;
            objects1[2] = sketchGeometricConstraint3;
            objects1[3] = sketchGeometricConstraint4;
            NXOpen.ErrorList errorList1;
            errorList1 = theSession.ActiveSketch.DeleteObjects(objects1);

            errorList1.Dispose();
            theSession.ActiveSketch.UpdateNavigator();

            // ----------------------------------------------
            //   Menu: Task->Finish Sketch
            // ----------------------------------------------
            NXOpen.SketchFindMovableObjectsBuilder sketchFindMovableObjectsBuilder2;
            sketchFindMovableObjectsBuilder2 = workPart.Sketches.CreateFindMovableObjectsBuilder();

            NXOpen.NXObject nXObject3;
            nXObject3 = sketchFindMovableObjectsBuilder2.Commit();

            sketchFindMovableObjectsBuilder2.Destroy();

            NXOpen.SketchWorkRegionBuilder sketchWorkRegionBuilder1;
            sketchWorkRegionBuilder1 = workPart.Sketches.CreateWorkRegionBuilder();

            sketchWorkRegionBuilder1.Scope = NXOpen.SketchWorkRegionBuilder.ScopeType.EntireSketch;

            NXOpen.NXObject nXObject4;
            nXObject4 = sketchWorkRegionBuilder1.Commit();

            sketchWorkRegionBuilder1.Destroy();

            theSession.ActiveSketch.CalculateStatus();

            theSession.Preferences.Sketch.SectionView = false;

            NXOpen.Session.UndoMarkId markId10;
            markId10 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Deactivate Sketch");

            theSession.ActiveSketch.Deactivate(NXOpen.Sketch.ViewReorient.True, NXOpen.Sketch.UpdateLevel.Model);

            theSession.DeleteUndoMarksSetInTaskEnvironment();

            theSession.EndTaskEnvironment();

            // ----------------------------------------------
            //   Menu: Insert->Design Feature->Extrude...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId11;
            markId11 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            NXOpen.Features.ExtrudeBuilder extrudeBuilder1;
            extrudeBuilder1 = workPart.Features.CreateExtrudeBuilder(nullNXOpen_Features_Feature);

            NXOpen.Section section1;
            section1 = workPart.Sections.CreateSection(0.0094999999999999998, 0.01, 0.5);

            extrudeBuilder1.Section = section1;

            extrudeBuilder1.AllowSelfIntersectingSection(true);

            NXOpen.Unit unit2;
            unit2 = extrudeBuilder1.Draft.FrontDraftAngle.Units;

            NXOpen.Expression expression7;
            expression7 = workPart.Expressions.CreateSystemExpressionWithUnits("2.00", unit2);

            extrudeBuilder1.DistanceTolerance = 0.01;

            NXOpen.Body[] targetBodies1 = new NXOpen.Body[1];
            NXOpen.Body nullNXOpen_Body = null;
            targetBodies1[0] = nullNXOpen_Body;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies1);

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            extrudeBuilder1.Limits.StartExtend.Value.SetFormula("500");

            extrudeBuilder1.Limits.EndExtend.Value.SetFormula("500");

            extrudeBuilder1.Draft.FrontDraftAngle.SetFormula("2");

            extrudeBuilder1.Draft.BackDraftAngle.SetFormula("2");

            extrudeBuilder1.Offset.StartOffset.SetFormula("0");

            extrudeBuilder1.Offset.EndOffset.SetFormula("5");

            extrudeBuilder1.Limits.StartExtend.Value.SetFormula("0");

            extrudeBuilder1.Limits.EndExtend.Value.SetFormula("500");

            NXOpen.GeometricUtilities.SmartVolumeProfileBuilder smartVolumeProfileBuilder1;
            smartVolumeProfileBuilder1 = extrudeBuilder1.SmartVolumeProfile;

            smartVolumeProfileBuilder1.OpenProfileSmartVolumeOption = false;

            smartVolumeProfileBuilder1.CloseProfileRule = NXOpen.GeometricUtilities.SmartVolumeProfileBuilder.CloseProfileRuleType.Fci;

            theSession.SetUndoMarkName(markId11, "Extrude Dialog");

            section1.DistanceTolerance = 0.01;

            section1.ChainingTolerance = 0.0094999999999999998;

            section1.SetAllowedEntityTypes(NXOpen.Section.AllowTypes.OnlyCurves);

            NXOpen.Session.UndoMarkId markId12;
            markId12 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, null);

            NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
            selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

            selectionIntentRuleOptions1.SetSelectedFromInactive(false);

            NXOpen.ICurve[] curves1 = new NXOpen.ICurve[4];
            curves1[0] = line1;
            curves1[1] = line2;
            curves1[2] = line3;
            curves1[3] = line4;
            NXOpen.Point3d seedPoint1 = new NXOpen.Point3d(28.678650000000001, 22.942920000000001, 0.0);
            NXOpen.RegionBoundaryRule regionBoundaryRule1;
            regionBoundaryRule1 = workPart.ScRuleFactory.CreateRuleRegionBoundary(sketch1, curves1, seedPoint1, 0.01, selectionIntentRuleOptions1);

            selectionIntentRuleOptions1.Dispose();
            section1.AllowSelfIntersection(true);

            section1.AllowDegenerateCurves(false);

            NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
            rules1[0] = regionBoundaryRule1;
            NXOpen.Point3d helpPoint1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
            section1.AddToSection(rules1, nullNXOpen_NXObject, nullNXOpen_NXObject, nullNXOpen_NXObject, helpPoint1, NXOpen.Section.Mode.Create, false);

            theSession.DeleteUndoMark(markId12, null);

            NXOpen.Session.UndoMarkId markId13;
            markId13 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "section mark");

            NXOpen.Session.UndoMarkId markId14;
            markId14 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, null);

            theSession.DeleteUndoMark(markId14, null);

            NXOpen.Direction direction2;
            direction2 = workPart.Directions.CreateDirection(sketch1, NXOpen.Sense.Forward, NXOpen.SmartObject.UpdateOption.WithinModeling);

            extrudeBuilder1.Direction = direction2;

            NXOpen.Body[] targetBodies2 = new NXOpen.Body[1];
            targetBodies2[0] = nullNXOpen_Body;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies2);

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            NXOpen.Body[] targetBodies3 = new NXOpen.Body[0];
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies3);

            theSession.DeleteUndoMark(markId13, null);

            NXOpen.Body[] targetBodies4 = new NXOpen.Body[1];
            targetBodies4[0] = nullNXOpen_Body;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies4);

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            NXOpen.Body[] targetBodies5 = new NXOpen.Body[0];
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies5);

            extrudeBuilder1.Limits.EndExtend.Value.SetFormula("3");

            NXOpen.Body[] targetBodies6 = new NXOpen.Body[1];
            targetBodies6[0] = nullNXOpen_Body;
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies6);

            extrudeBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;

            NXOpen.Body[] targetBodies7 = new NXOpen.Body[0];
            extrudeBuilder1.BooleanOperation.SetTargetBodies(targetBodies7);

            NXOpen.Session.UndoMarkId markId15;
            markId15 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Extrude");

            theSession.DeleteUndoMark(markId15, null);

            NXOpen.Session.UndoMarkId markId16;
            markId16 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Extrude");

            extrudeBuilder1.ParentFeatureInternal = false;

            NXOpen.Features.Feature feature2;
            feature2 = extrudeBuilder1.CommitFeature();

            theSession.DeleteUndoMark(markId16, null);

            theSession.SetUndoMarkName(markId11, "Extrude");

            NXOpen.Expression expression8 = extrudeBuilder1.Limits.StartExtend.Value;
            NXOpen.Expression expression9 = extrudeBuilder1.Limits.EndExtend.Value;
            
            workPart.Expressions.Delete(expression7);

            Body[] bodies = feature2.GetBodies();

            extrudeBuilder1.Destroy();

            return bodies[0];
        }

        private static PointFeature[] CreateHolePoints(Part workPart, Point3d[] coordinates)
        {
            List<PointFeature> pointFeatures = new List<PointFeature>();

            foreach (Point3d coordinate in coordinates)
            {
                Point point = workPart.Points.CreatePoint(coordinate);
                PointFeatureBuilder pointBuilder = workPart.BaseFeatures.CreatePointFeatureBuilder(null);

                pointBuilder.Point = point;
                PointFeature pointFeature = (PointFeature)pointBuilder.Commit();

                pointBuilder.Destroy();
                pointFeatures.Add(pointFeature);
            }
            return pointFeatures.ToArray();
        }

        private static void CreateThroughHoles(Session theSession, Part workPart, Body plateBody, PointFeature[] pointFeatures, double diameter)
        {
            HolePackageBuilder holeBuilder = workPart.Features.CreateHolePackageBuilder(null);

            holeBuilder.Tolerance = 0.01;
            holeBuilder.GeneralSimpleHoleDiameter.SetFormula(diameter.ToString(System.Globalization.CultureInfo.InvariantCulture));

            holeBuilder.HoleDepthLimitOption = HolePackageBuilder.HoleDepthLimitOptions.ThroughBody;

            holeBuilder.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Subtract;

            ScCollector targetCollector = holeBuilder.BooleanOperation.GetTargetBodiesCollector();
            SelectionIntentRuleOptions bodyRuleOptions = workPart.ScRuleFactory.CreateRuleOptions();
            bodyRuleOptions.SetSelectedFromInactive(false);

            BodyDumbRule bodyRule = workPart.ScRuleFactory.CreateRuleBodyDumb(
                    new Body[] { plateBody },
                    true,
                    bodyRuleOptions);

            bodyRuleOptions.Dispose();

            targetCollector.ReplaceRules(new SelectionIntentRule[] { bodyRule }, false);

            holeBuilder.HolePosition.DistanceTolerance = 0.01;
            holeBuilder.HolePosition.ChainingTolerance = 0.0095;

            holeBuilder.HolePosition.SetAllowedEntityTypes(Section.AllowTypes.OnlyPoints);

            FeaturePointsRule pointRule = workPart.ScRuleFactory.CreateRuleFeaturePoints(pointFeatures, null);

            holeBuilder.HolePosition.AllowSelfIntersection(true);
            holeBuilder.HolePosition.AllowDegenerateCurves(false);

            holeBuilder.HolePosition.AddToSection(
                new SelectionIntentRule[] { pointRule },
                null,
                null,
                null,
                new Point3d(0.0, 0.0, 0.0),
                Section.Mode.Create,
                false);

            holeBuilder.Commit();
            holeBuilder.Destroy();
        }

        public static int GetUnloadOption(string dummy)
        {
            return (int)Session.LibraryUnloadOption.Immediately;
        }
    }
}
