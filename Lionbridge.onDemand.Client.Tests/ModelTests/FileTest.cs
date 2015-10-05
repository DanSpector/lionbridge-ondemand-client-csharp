/* Copyright (c) 2015 Lionbridge Technologies, Inc.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    /// <summary>
    /// This class contains unit tests for the File class
    /// </summary>
    [TestClass]
    public class FileTest
    {
        [TestMethod]
        public void ConstructorFromXmlTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <File>
                    <AssetID>123456</AssetID>
                    <Name>Foo.txt</Name>
                    <Status>Analyzed</Status>
                    <SourceLanguage>
                        <LanguageCode>en-gb</LanguageCode>
                    </SourceLanguage>
                    <ProjectID>10001</ProjectID>
                </File>
            ";

            XDocument document = XDocument.Parse(xml);

            var file = new File(document.Element("File"), new MockContentAPI());

            Assert.AreEqual(123456, file.AssetID);
            Assert.AreEqual("Foo.txt", file.Name);
            Assert.AreEqual(FileStatus.Analyzed, file.Status);
            Assert.AreEqual(10001, file.ProjectID);
            Assert.IsNotNull(file.SourceLanguage);
            Assert.AreEqual("en-gb", file.SourceLanguage.LanguageCode);
        }

        [TestMethod]
        public void CreateEnumerableTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Files>
                    <File>
                        <AssetID>9000</AssetID>
                        <Status>New</Status>
                        <URL>http://www.lionbridge.com</URL>
                        <Name>1.txt</Name>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <UploadDate>2014-01-25T10:32:02Z</UploadDate>
                    </File>
                    <File>
                        <AssetID>9900</AssetID>
                        <Status>Analyzed</Status>
                        <URL>http://www.lionbridge.com</URL>
                        <Name>1.txt</Name>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <UploadDate>2014-01-25T10:32:02Z</UploadDate>
                    </File>
                    <File>
                        <AssetID>9901</AssetID>
                        <Status>Analysis Failed</Status>
                        <URL>http://www.lionbridge.com</URL>
                        <Name>1.txt</Name>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <UploadDate>2014-01-25T10:32:02Z</UploadDate>
                    </File>
                    <File>
                        <AssetID>9910</AssetID>
                        <Status>In Translation</Status>
                        <ProjectID>1234</ProjectID>
                        <URL>https://www.lionbridge.com</URL>
                        <Name>foo.txt</Name>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <UploadDate>2014-01-25T10:32:02Z</UploadDate>
                        <TargetLanguages>
                            <TargetLanguage>
                                <LanguageCode>de-de</LanguageCode>
                                <Status>Started</Status>
                                <ProjectURL>https://www.lionbridge.com</ProjectURL>
                            </TargetLanguage>
                            <TargetLanguage>
                                <LanguageCode>fr-fr</LanguageCode>
                                <Status>Started</Status>
                                <ProjectURL>https://www.lionbridge.com</ProjectURL>
                            </TargetLanguage>
                        </TargetLanguages>
                    </File>
                    <File>
                        <AssetID>9999</AssetID>
                        <Status>Translated</Status>
                        <ProjectID>1234</ProjectID>
                        <URL>https://www.lionbridge.com</URL>
                        <Name>foo.txt</Name>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <UploadDate>2014-01-25T10:32:02Z</UploadDate>
                        <TargetLanguages>
                            <TargetLanguage>
                                <LanguageCode>de-de</LanguageCode>
                                <Status>Complete</Status>
                                <ProjectURL>https://www.lionbridge.com</ProjectURL>
                                <DownloadURL>https://ondemand.lionbridge.com</DownloadURL>
                            </TargetLanguage>
                            <TargetLanguage>
                                <LanguageCode>fr-fr</LanguageCode>
                                <Status>Complete</Status>
                                <ProjectURL>https://www.lionbridge.com</ProjectURL>
                                <DownloadURL>https://ondemand.lionbridge.com</DownloadURL>
                            </TargetLanguage>
                        </TargetLanguages>
                    </File>
                </Files>
            ";

            XDocument document = XDocument.Parse(xml);

            IEnumerable<File> files = File.CreateEnumerable(document.Element("Files"), new MockContentAPI());

            Assert.IsNotNull(files);
            Assert.AreEqual(5, files.Count());

            // File 1
            var file = files.ElementAt(0);
            Assert.AreEqual(9000, file.AssetID);
            Assert.AreEqual("1.txt", file.Name);
            Assert.AreEqual(FileStatus.New, file.Status);
            Assert.IsNotNull(file.SourceLanguage);
            Assert.AreEqual("en-gb", file.SourceLanguage.LanguageCode);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), file.UploadDate);
            Assert.AreEqual(new Uri("http://www.lionbridge.com"), file.URL);

            // File 2
            file = files.ElementAt(1);
            Assert.AreEqual(9900, file.AssetID);
            Assert.AreEqual("1.txt", file.Name);
            Assert.AreEqual(FileStatus.Analyzed, file.Status);
            Assert.IsNotNull(file.SourceLanguage);
            Assert.AreEqual("en-gb", file.SourceLanguage.LanguageCode);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), file.UploadDate);
            Assert.AreEqual(new Uri("http://www.lionbridge.com"), file.URL);

            // File 3
            file = files.ElementAt(2);
            Assert.AreEqual(9901, file.AssetID);
            Assert.AreEqual("1.txt", file.Name);
            Assert.AreEqual(FileStatus.AnalysisFailed, file.Status);
            Assert.IsNotNull(file.SourceLanguage);
            Assert.AreEqual("en-gb", file.SourceLanguage.LanguageCode);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), file.UploadDate);
            Assert.AreEqual(new Uri("http://www.lionbridge.com"), file.URL);

            // File 4
            file = files.ElementAt(3);
            Assert.AreEqual(9910, file.AssetID);
            Assert.AreEqual(1234, file.ProjectID);
            Assert.AreEqual("foo.txt", file.Name);
            Assert.AreEqual(FileStatus.InTranslation, file.Status);
            Assert.IsNotNull(file.SourceLanguage);
            Assert.AreEqual("en-gb", file.SourceLanguage.LanguageCode);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), file.UploadDate);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), file.URL);

            Assert.IsNotNull(file.TargetLanguages);
            Assert.AreEqual(2, file.TargetLanguages.Count());
            Assert.AreEqual("de-de", file.TargetLanguages.ElementAt(0).LanguageCode);
            Assert.AreEqual(TranslatedFileStatus.Started, file.TargetLanguages.ElementAt(0).Status);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), file.TargetLanguages.ElementAt(0).ProjectURL);


            Assert.AreEqual("fr-fr", file.TargetLanguages.ElementAt(1).LanguageCode);
            Assert.AreEqual(TranslatedFileStatus.Started, file.TargetLanguages.ElementAt(1).Status);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), file.TargetLanguages.ElementAt(1).ProjectURL);

            // File 5
            file = files.ElementAt(4);
            Assert.AreEqual(9999, file.AssetID);
            Assert.AreEqual(1234, file.ProjectID);
            Assert.AreEqual("foo.txt", file.Name);
            Assert.AreEqual(FileStatus.Translated, file.Status);
            Assert.IsNotNull(file.SourceLanguage);
            Assert.AreEqual("en-gb", file.SourceLanguage.LanguageCode);
            Assert.AreEqual(DateTime.Parse("2014-01-25T10:32:02Z"), file.UploadDate);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), file.URL);

            Assert.IsNotNull(file.TargetLanguages);
            Assert.AreEqual(2, file.TargetLanguages.Count());
            Assert.AreEqual("de-de", file.TargetLanguages.ElementAt(0).LanguageCode);
            Assert.AreEqual(TranslatedFileStatus.Complete, file.TargetLanguages.ElementAt(0).Status);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), file.TargetLanguages.ElementAt(0).ProjectURL);
            Assert.AreEqual(new Uri("https://ondemand.lionbridge.com"), file.TargetLanguages.ElementAt(0).DownloadURL);


            Assert.AreEqual("fr-fr", file.TargetLanguages.ElementAt(1).LanguageCode);
            Assert.AreEqual(TranslatedFileStatus.Complete, file.TargetLanguages.ElementAt(1).Status);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), file.TargetLanguages.ElementAt(1).ProjectURL);
            Assert.AreEqual(new Uri("https://ondemand.lionbridge.com"), file.TargetLanguages.ElementAt(1).DownloadURL);
        }

        [TestMethod]
        public void CreateEnumerableReferenceFileTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <ReferenceFiles>
                    <ReferenceFile>
                        <AssetID>123456</AssetID>
                        <Name>Foo.txt</Name>
                    </ReferenceFile>
                    <ReferenceFile>
                        <AssetID>123456</AssetID>
                        <Name>Foo.txt</Name>
                        
                    </ReferenceFile>
                </ReferenceFiles>
            ";

            XDocument document = XDocument.Parse(xml);

            IEnumerable<File> files = File.CreateEnumerable(document.Element("ReferenceFiles"), new MockContentAPI());

            Assert.IsNotNull(files);
            Assert.AreEqual(2, files.Count());
        }

        [TestMethod]
        public void ToXmlStringTest()
        {
            var file = new File() { AssetID = 123456 };

            var xml = @"
                <File>
                    <AssetID>123456</AssetID>
                </File>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), file.ToXmlString());
        }

        [TestMethod]
        public void ToXmlStringExtendedReferenceFileTest()
        {
            var file = new File() { AssetID = 123456 };

            var xml = @"
                <ReferenceFile>
                    <AssetID>123456</AssetID>
                </ReferenceFile>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), file.ToXmlStringExtended());
        }

        [TestMethod]
        public void ToXmlStringExtendedTest()
        {
            var file = new File() { AssetID = 123456 };

            var xml = @"
                <File>
                    <AssetID>123456</AssetID>
                    <FileName />
                    <URL />
                    <TargetLanguages></TargetLanguages>
                </File>
            ";

            Assert.AreEqual(Regex.Replace(xml, @"\s", ""), Regex.Replace(file.ToXmlStringExtended(isReferenceFile: false, includeExtended: true), @"\s", ""));

            var file2 = new File() { AssetID = 123456, ProjectID = 34567 };

            var xml2 = @"
                <File>
                    <AssetID>123456</AssetID>
                    <FileName />
                    <URL />
                    <ProjectID>34567</ProjectID>
                    <TargetLanguages></TargetLanguages>
                </File>
            ";

            Assert.AreEqual(Regex.Replace(xml2, @"\s", ""), Regex.Replace(file2.ToXmlStringExtended(isReferenceFile: false, includeExtended: true), @"\s", ""));
        }
    }
}
