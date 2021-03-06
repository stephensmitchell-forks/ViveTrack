﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using ViveTrack.Properties;

namespace ViveTrack
{
    public class CalibrateOrigin : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CalibrateCenter class.
        /// </summary>

        

        private bool reset;
        private bool calibrate;

        public CalibrateOrigin()
          : base("CalibrateOrigin", "CalibrateOrigin",
              "Reorient all the traced devices according to the new plane you set as origin plane\n\n" +
              "WARNING: \n\n" +
              "If you want to recalibrate according to another plane, please first reset then click calibrate.",
              "ViveTrack", "ViveTrack")
        {
            reset = false;
            calibrate = false;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("OriginPlane", "OriginPlane", "The new designated origin plane",GH_ParamAccess.item);
            pManager.AddBooleanParameter("Calibrate", "Calibrate","Add a boolean button here, click to calibrate origin", GH_ParamAccess.item,false);
            pManager.AddBooleanParameter("Reset", "Reset", "Reset the calibration data to none", GH_ParamAccess.item,false);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.AddMatrixParameter("Matrix", "Matrix", "The transformation matrix between old plane and new plane.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Plane tempPlane = new GH_Plane();
            if(!DA.GetData("OriginPlane", ref tempPlane))return;
            
            DA.GetData("Calibrate", ref calibrate);
            DA.GetData("Reset", ref reset);



            if (calibrate)
            {
                if (StartVive.CalibrationPlane == null) StartVive.CalibrationPlane = tempPlane;
                Plane iPlane = StartVive.CalibrationPlane.Value;
                Plane xyPlane = Plane.WorldXY;
                StartVive.CalibrationTransform = Transform.ChangeBasis(xyPlane, iPlane);
            };

            if (reset)
            {
                StartVive.CalibrationTransform = Transform.Identity;
                StartVive.CalibrationPlane = null;
            }

            //DA.SetData("Matrix", CalibrationTransform);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.Calibration;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dac6245e-a351-4f9a-8dbd-e18726495f0e"); }
        }

    }
}