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
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lionbridge.onDemand.Client.Tests
{
    [TestClass]
    public class ProjectTest
    {
        [TestMethod]
        public void ConstructorFromXmlTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Project>
                    <ProjectID>123</ProjectID>
                    <ProjectName>Name of project</ProjectName>
                    <ProjectURL>https://www.lionbridge.com</ProjectURL>
                    <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                    <Price>100.00</Price>
                    <Currency>USD</Currency>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <CompletionDate>2014-01-25T10:32:02Z</CompletionDate>
                    <ServiceID>54</ServiceID>
                    <SourceLanguage>
                        <LanguageCode>en-gb</LanguageCode>
                    </SourceLanguage>
                    <TargetLanguages>
                        <TargetLanguage>
                            <LanguageCode>it-it</LanguageCode>
                        </TargetLanguage>
                        <TargetLanguage>
                            <LanguageCode>fr-fr</LanguageCode>
                        </TargetLanguage>
                    </TargetLanguages>
                    <Products>
                        <Product>
                            <AssetID>999</AssetID>
                            <SKUs>
                                <SKU>
                                    <SKUNumber>123</SKUNumber>
                                </SKU>
                            </SKUs>
                        </Product>
                    </Products>
                    <Files>
                            <File>
                                <AssetID>999</AssetID>
                                <FileName>example.txt</FileName>
                            </File>
                    </Files>
                    <ReferenceFiles>
                        <ReferenceFile>
                            <AssetID>12345</AssetID>
                            <FileName>my-file.txt</FileName>
                            <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                            <TargetLanguages />
                        </ReferenceFile>
                    </ReferenceFiles>
                </Project>
            ";

            XDocument document = XDocument.Parse(xml);

            var project = new Project(document.Element("Project"), new MockContentAPI());

            Assert.AreEqual(123, project.ProjectID);
            Assert.AreEqual("Name of project", project.Name);
            Assert.AreEqual(new Uri("https://www.lionbridge.com"), project.URL);
            Assert.AreEqual(54, project.ServiceID);
            Assert.AreEqual(DateTime.Parse("2014-02-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual(100.00m, project.Price);
            Assert.AreEqual("USD", project.Currency);
            Assert.AreEqual("en-gb", project.SourceLanguage);
            Assert.AreEqual(2, project.TargetLanguages.Count());
            Assert.AreEqual("it-it", project.TargetLanguages.FirstOrDefault());
            Assert.AreEqual("fr-fr", project.TargetLanguages.LastOrDefault());

            Assert.IsNotNull(project.Products);
            Assert.AreEqual(1, project.Products.Count());

            var product = project.Products.FirstOrDefault();

            Assert.AreEqual(999, product.AssetID);
            Assert.AreEqual(1, product.SKUs.Count());
            Assert.AreEqual("123", product.SKUs.FirstOrDefault().SKUNumber);

            Assert.IsNotNull(project.Files);
            Assert.AreEqual(1, project.Files.Count());

            Assert.AreEqual(999, project.Files.FirstOrDefault().AssetID);
            Assert.AreEqual("example.txt", project.Files.FirstOrDefault().Name);

            Assert.IsNotNull(project.ReferenceFiles);
            Assert.AreEqual(1, project.ReferenceFiles.Count());

            Assert.AreEqual(12345, project.ReferenceFiles.FirstOrDefault().AssetID);
            Assert.AreEqual("my-file.txt", project.ReferenceFiles.FirstOrDefault().Name);
            Assert.AreEqual(new Uri("https://ondemand.liondemand.com/api/files/12345"), project.ReferenceFiles.FirstOrDefault().URL);
            Assert.AreEqual(0, project.ReferenceFiles.FirstOrDefault().TargetLanguages.Count());
        }

        [TestMethod]
        public void CreateEnumerableTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Projects>
                    <Project>
                        <ProjectID>123</ProjectID>
                        <ProjectName>Name of project</ProjectName>
                        <ProjectURL>https://</ProjectURL>
                        <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                        <Price>100.00</Price>
                        <Currency>USD</Currency>
                        <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                        <CompletionDate>2014-01-25T10:32:02Z</CompletionDate>
                        <ServiceID>54</ServiceID>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <TargetLanguages>
                            <TargetLanguage>
                                <LanguageCode>it-it</LanguageCode>
                            </TargetLanguage>
                            <TargetLanguage>
                                <LanguageCode>fr-fr</LanguageCode>
                            </TargetLanguage>
                        </TargetLanguages>
                        <Products>
                            <Product>
                                <AssetID>999</AssetID>
                                <SKUs>
                                    <SKU>
                                        <SKUNumber>123</SKUNumber>
                                    </SKU>
                                </SKUs>
                            </Product>
                        </Products>
                        <Files>
                                <File>
                                    <AssetID>999</AssetID>
                                    <FileName>example.txt</FileName>
                                </File>
                        </Files>
                        <ReferenceFiles>
                            <ReferenceFile>
                                <AssetID>12345</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                <TargetLanguages />
                            </ReferenceFile>
                        </ReferenceFiles>
                    </Project>
                    <Project>
                        <ProjectID>123</ProjectID>
                        <ProjectName>Name of project</ProjectName>
                        <ProjectURL>https://</ProjectURL>
                        <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                        <Price>100.00</Price>
                        <Currency>USD</Currency>
                        <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                        <CompletionDate>2014-01-25T10:32:02Z</CompletionDate>
                        <ServiceID>54</ServiceID>
                        <SourceLanguage>
                            <LanguageCode>en-gb</LanguageCode>
                        </SourceLanguage>
                        <TargetLanguages>
                            <TargetLanguage>
                                <LanguageCode>it-it</LanguageCode>
                            </TargetLanguage>
                            <TargetLanguage>
                                <LanguageCode>fr-fr</LanguageCode>
                            </TargetLanguage>
                        </TargetLanguages>
                        <Products>
                            <Product>
                                <AssetID>999</AssetID>
                                <SKUs>
                                    <SKU>
                                        <SKUNumber>123</SKUNumber>
                                    </SKU>
                                </SKUs>
                            </Product>
                        </Products>
                        <Files>
                                <File>
                                    <AssetID>999</AssetID>
                                    <FileName>example.txt</FileName>
                                </File>
                        </Files>
                        <ReferenceFiles>
                            <ReferenceFile>
                                <AssetID>12345</AssetID>
                                <FileName>my-file.txt</FileName>
                                <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                                <TargetLanguages />
                            </ReferenceFile>
                        </ReferenceFiles>
                    </Project>
                </Projects>
            ";

            XDocument document = XDocument.Parse(xml);

            IEnumerable<Project> projects = Project.CreateEnumerable(document.Element("Projects"), new MockContentAPI());

            Assert.IsNotNull(projects);
            Assert.AreEqual(2, projects.Count());

        }

        [TestMethod]
        public void UpdateTest()
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
                <Project>
                    <ProjectID>123</ProjectID>
                    <ProjectName>Name of project</ProjectName>
                    <ProjectURL>https://</ProjectURL>
                    <ProjectDueDate>2014-02-11T10:22:46Z</ProjectDueDate>
                    <Price>100.00</Price>
                    <Currency>USD</Currency>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <CompletionDate>2014-01-25T10:32:02Z</CompletionDate>
                    <ServiceID>54</ServiceID>
                    <SourceLanguage>
                        <LanguageCode>en-gb</LanguageCode>
                    </SourceLanguage>
                    <TargetLanguages>
                        <TargetLanguage>
                            <LanguageCode>it-it</LanguageCode>
                        </TargetLanguage>
                        <TargetLanguage>
                            <LanguageCode>fr-fr</LanguageCode>
                        </TargetLanguage>
                    </TargetLanguages>
                    <Products>
                        <Product>
                            <AssetID>999</AssetID>
                            <SKUs>
                                <SKU>
                                    <SKUNumber>123</SKUNumber>
                                </SKU>
                            </SKUs>
                        </Product>
                    </Products>
                    <Files>
                            <File>
                                <AssetID>999</AssetID>
                                <FileName>example.txt</FileName>
                            </File>
                    </Files>
                    <ReferenceFiles>
                        <ReferenceFile>
                            <AssetID>12345</AssetID>
                            <FileName>my-file.txt</FileName>
                            <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                            <TargetLanguages />
                        </ReferenceFile>
                    </ReferenceFiles>
                </Project>
            ";

            XDocument document = XDocument.Parse(xml);

            var project = new Project(document.Element("Project"), new MockContentAPI());

            string xml2 = @"<?xml version='1.0' encoding='UTF-8'?>
                <Project>
                    <ProjectID>123</ProjectID>
                    <ProjectName>New Name</ProjectName>
                    <ProjectURL></ProjectURL>
                    <DueDate>2014-03-11T10:22:46Z</DueDate>
                    <Price>100.00</Price>
                    <Currency>USD</Currency>
                    <CreationDate>2014-01-25T10:32:02Z</CreationDate>
                    <CompletionDate>2014-01-25T10:32:02Z</CompletionDate>
                    <ServiceID>54</ServiceID>
                    <SourceLanguage>
                        <LanguageCode>en-gb</LanguageCode>
                    </SourceLanguage>
                    <TargetLanguages>
                        <TargetLanguage>
                            <LanguageCode>it-it</LanguageCode>
                        </TargetLanguage>
                        <TargetLanguage>
                            <LanguageCode>fr-fr</LanguageCode>
                        </TargetLanguage>
                    </TargetLanguages>
                    <Products>
                        <Product>
                            <AssetID>999</AssetID>
                            <SKUs>
                                <SKU>
                                    <SKUNumber>123</SKUNumber>
                                </SKU>
                            </SKUs>
                        </Product>
                    </Products>
                    <Files>
                            <File>
                                <AssetID>999</AssetID>
                                <FileName>example.txt</FileName>
                            </File>
                    </Files>
                    <ReferenceFiles>
                        <ReferenceFile>
                            <AssetID>12345</AssetID>
                            <FileName>my-file.txt</FileName>
                            <URL>https://ondemand.liondemand.com/api/files/12345</URL>
                            <TargetLanguages />
                        </ReferenceFile>
                    </ReferenceFiles>
                </Project>
            ";

            XDocument document2 = XDocument.Parse(xml2);

            project.UpdateFromXElement(document2.Element("Project"));

            Assert.AreEqual(DateTime.Parse("2014-03-11T10:22:46Z"), project.DueDate);
            Assert.AreEqual("New Name", project.Name);
        }
    }
}
