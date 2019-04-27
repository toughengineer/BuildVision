﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;

namespace BuildVision.Contracts
{
    public class ErrorItem
    {
        public const string DefaultSortPropertyName = "Number";
        private readonly List<string> _invalidFileNames = new List<string> { "CSC", "MSBUILD", "LINK" };

        public bool CanNavigateTo { get; set; }

        public int? Number { get; set; }

        public ErrorLevel Level { get; set; }

        public string Code { get; set; }

        public string File { get; set; }

        public string FileName
        {
            get
            {
                try
                {
                    return Path.GetFileName(File);
                }
                catch (ArgumentException)
                {
                    return File;
                }
            }
        }

        public string ProjectFile { get; set; }

        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

        public int EndLineNumber { get; set; }

        public int EndColumnNumber { get; set; }

        public string Subcategory { get; set; }

        public string Message { get; set; }

        public ErrorItem(ErrorLevel errorLevel)
        {
            Level = errorLevel;
        }

        public ErrorItem() { }

        public void Init(BuildErrorEventArgs e)
        {
            Code = e.Code;
            File = e.File;
            ProjectFile = e.ProjectFile;
            LineNumber = e.LineNumber;
            ColumnNumber = e.ColumnNumber;
            EndLineNumber = e.EndLineNumber;
            EndColumnNumber = e.EndColumnNumber;
            Subcategory = e.Subcategory;
            Message = e.Message;
        }

        public void Init(BuildWarningEventArgs e)
        {
            Code = e.Code;
            File = e.File;
            ProjectFile = e.ProjectFile;
            LineNumber = e.LineNumber;
            ColumnNumber = e.ColumnNumber;
            EndLineNumber = e.EndLineNumber;
            EndColumnNumber = e.EndColumnNumber;
            Subcategory = e.Subcategory;
            Message = e.Message;
        }

        public void Init(BuildMessageEventArgs e)
        {
            Code = e.Code;
            File = e.File;
            ProjectFile = e.ProjectFile;
            LineNumber = e.LineNumber;
            ColumnNumber = e.ColumnNumber;
            EndLineNumber = e.EndLineNumber;
            EndColumnNumber = e.EndColumnNumber;
            Subcategory = e.Subcategory;
            Message = e.Message;
        }

        // 1. EnvDTE.TextSelection.MoveToLineAndOffset requires line and offset numbers beginning at one.
        // BuildErrorEventArgs.LineNumber and BuildErrorEventArgs.ColumnNumber may be uninitialized.
        // 2. BuildErrorEventArgs.EndLineNumber and BuildErrorEventArgs.EndColumnNumber may be uninitialized,
        // regardless of BuildErrorEventArgs.LineNumber and BuildErrorEventArgs.ColumnNumber.
        public void VerifyValues()
        {
            if (_invalidFileNames.Contains(File) && LineNumber == 0 && ColumnNumber == 0)
            {
                CanNavigateTo = false;
                return;
            }

            if (LineNumber < 1)
            {
                LineNumber = 1;
            }

            if (ColumnNumber < 1)
            {
                ColumnNumber = 1;
            }

            if (EndLineNumber == 0 && EndColumnNumber == 0)
            {
                EndLineNumber = LineNumber;
                EndColumnNumber = ColumnNumber;
            }

            CanNavigateTo = true;
        }
    }
}
