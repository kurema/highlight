﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// このソース コードは xsd によって自動生成されました。Version=4.8.3928.0 です。
// 
namespace Highlight.Definitions {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class patternType {
        
        private fontType fontField;
        
        private object[] wordField;
        
        private string nameField;
        
        private constructType typeField;
        
        private string beginsWithField;
        
        private string endsWithField;
        
        private string escapesWithField;
        
        private bool highlightAttributesField;
        
        private bool highlightAttributesFieldSpecified;
        
        /// <remarks/>
        public fontType font {
            get {
                return this.fontField;
            }
            set {
                this.fontField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("word")]
        public object[] word {
            get {
                return this.wordField;
            }
            set {
                this.wordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public constructType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string beginsWith {
            get {
                return this.beginsWithField;
            }
            set {
                this.beginsWithField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string endsWith {
            get {
                return this.endsWithField;
            }
            set {
                this.endsWithField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string escapesWith {
            get {
                return this.escapesWithField;
            }
            set {
                this.escapesWithField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool highlightAttributes {
            get {
                return this.highlightAttributesField;
            }
            set {
                this.highlightAttributesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool highlightAttributesSpecified {
            get {
                return this.highlightAttributesFieldSpecified;
            }
            set {
                this.highlightAttributesFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fontType {
        
        private styleType bracketStyleField;
        
        private styleType attributeNameStyleField;
        
        private styleType attributeValueStyleField;
        
        private string nameField;
        
        private string sizeField;
        
        private string styleField;
        
        private string foreColorField;
        
        private string backColorField;
        
        /// <remarks/>
        public styleType bracketStyle {
            get {
                return this.bracketStyleField;
            }
            set {
                this.bracketStyleField = value;
            }
        }
        
        /// <remarks/>
        public styleType attributeNameStyle {
            get {
                return this.attributeNameStyleField;
            }
            set {
                this.attributeNameStyleField = value;
            }
        }
        
        /// <remarks/>
        public styleType attributeValueStyle {
            get {
                return this.attributeValueStyleField;
            }
            set {
                this.attributeValueStyleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
        public string size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string style {
            get {
                return this.styleField;
            }
            set {
                this.styleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string foreColor {
            get {
                return this.foreColorField;
            }
            set {
                this.foreColorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string backColor {
            get {
                return this.backColorField;
            }
            set {
                this.backColorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class styleType {
        
        private string foreColorField;
        
        private string backColorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string foreColor {
            get {
                return this.foreColorField;
            }
            set {
                this.foreColorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string backColor {
            get {
                return this.backColorField;
            }
            set {
                this.backColorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    public enum constructType {
        
        /// <remarks/>
        block,
        
        /// <remarks/>
        word,
        
        /// <remarks/>
        markup,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class definitionType {
        
        private defaultType defaultField;
        
        private patternType[] patternField;
        
        private string nameField;
        
        private bool caseSensitiveField;
        
        /// <remarks/>
        public defaultType @default {
            get {
                return this.defaultField;
            }
            set {
                this.defaultField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pattern")]
        public patternType[] pattern {
            get {
                return this.patternField;
            }
            set {
                this.patternField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool caseSensitive {
            get {
                return this.caseSensitiveField;
            }
            set {
                this.caseSensitiveField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class defaultType {
        
        private fontType fontField;
        
        /// <remarks/>
        public fontType font {
            get {
                return this.fontField;
            }
            set {
                this.fontField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class definitions {
        
        private definitionType[] definitionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("definition")]
        public definitionType[] definition {
            get {
                return this.definitionField;
            }
            set {
                this.definitionField = value;
            }
        }
    }
}
