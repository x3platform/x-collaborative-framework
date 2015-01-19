// -*- ecoding=utf-8 -*-
// Name     : x-client-api 
// Version  : 1.0.0 
// Author   : ruanyu@live.com
// Date     : 2014-11-15
(function(global, factory) 
{
    if (typeof module === "object" && typeof module.exports === "object") 
    {
        module.exports = global.document ?
        factory(global, true) :
        function(w) 
        {
            if (!w.document) { throw new Error("requires a window with a document"); }

            return factory(w);
        };
    } 
    else 
    {
        factory(global);
    }
} (typeof window !== "undefined" ? window : this, function(window, noGlobal) {

    // -------------------------------------------------------
    // 扩展 Function 对象方法
    // -------------------------------------------------------
    
    if (!Function.prototype.bind)
    {
        Function.prototype.bind = function(that)
        {
            if (typeof this !== "function")
            {
                // closest thing possible to the ECMAScript 5 internal
                // IsCallable function
                throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
            }
    
            var args = Array.prototype.slice.call(arguments, 1),
    		    me = this,
    			NOP = function() { },
    			bound = function()
    			{
    			    return me.apply(this instanceof NOP
    					    ? this
    					    : that || window,
    					    args.concat(Array.prototype.slice.call(arguments)));
    			};
    
            NOP.prototype = this.prototype;
            bound.prototype = new NOP();
    
            return bound;
        };
    };
    
    // -------------------------------------------------------
    // 扩展 String 对象方法
    // -------------------------------------------------------
    
    /*#region 类:String*/
    /**
    * 创建 String 对象
    * @class String 扩展 Javascript 的 String 对象的方法
    * @constructor String
    */
    
    /*#region 函数:trim()*/
    if (!String.prototype.trim)
    {
        /**
        * 去除字符串两侧空白
        * @method trim
        * @memberof String#
        * @returns {string}
        */
        String.prototype.trim = function()
        {
            return this.replace(/(^\s*)|(\s*$)/g, '');
        }
    };
    /*#endregion*/
    
    /*#region 函数:ltrim()*/
    if (!String.prototype.ltrim)
    {
        /**
        * 去除字符串左侧空白
        * @method ltrim
        * @memberof String#
        * @returns {string}
        */
        String.prototype.ltrim = function()
        {
            return this.replace(/(^\s*)/g, '');
        }
    };
    /*#endregion*/
    
    /*#region 函数:rtrim()*/
    if (!String.prototype.rtrim)
    {
        /**
        * 去除字符串右侧空白
        * @method rtrim
        * @memberof String#
        * @returns {string}
        */
        String.prototype.rtrim = function()
        {
            return this.replace(/(\s*$)/g, '');
        }
    };
    /*#endregion*/
    
    /*#region 函数:exists()*/
    if (!String.prototype.exists)
    {
        /**
        * 利用正则表达式验证字符串规则
        * @method exists
        * @memberof String#
        * @param {RegExp} regexp 正则表达式
        * @returns {bool}
        */
        String.prototype.exists = function(regexp)
        {
            return this.match(regexp) !== null;
        }
    };
    /*#endregion*/
    
    /*#region 函数:format()*/
    if (!String.prototype.format)
    {
        String.prototype.format = function()
        {
            var args = arguments;
    
            return this.replace(/\{(\d+)\}/g, function(m, i)
            {
                return args[i];
            });
        }
    }
    /*#endregion*/
    
    /*#endregion*/
    /*!
     * Sizzle CSS Selector Engine v2.0.1-pre
     * http://sizzlejs.com/
     *
     * Copyright 2008, 2014 jQuery Foundation, Inc. and other contributors
     * Released under the MIT license
     * http://jquery.org/license
     *
     * Date: 2014-07-01
     */
    (function( window ) {
    
    var i,
    	support,
    	Expr,
    	getText,
    	isXML,
    	tokenize,
    	compile,
    	select,
    	outermostContext,
    	sortInput,
    	hasDuplicate,
    
    	// Local document vars
    	setDocument,
    	document,
    	docElem,
    	documentIsHTML,
    	rbuggyQSA,
    	rbuggyMatches,
    	matches,
    	contains,
    
    	// Instance-specific data
    	expando = "sizzle" + -(new Date()),
    	preferredDoc = window.document,
    	dirruns = 0,
    	done = 0,
    	classCache = createCache(),
    	tokenCache = createCache(),
    	compilerCache = createCache(),
    	sortOrder = function( a, b ) {
    		if ( a === b ) {
    			hasDuplicate = true;
    		}
    		return 0;
    	},
    
    	// General-purpose constants
    	strundefined = typeof undefined,
    	MAX_NEGATIVE = 1 << 31,
    
    	// Instance methods
    	hasOwn = ({}).hasOwnProperty,
    	arr = [],
    	pop = arr.pop,
    	push_native = arr.push,
    	push = arr.push,
    	slice = arr.slice,
    	// Use a stripped-down indexOf if we can't use a native one
    	indexOf = arr.indexOf || function( elem ) {
    		var i = 0,
    			len = this.length;
    		for ( ; i < len; i++ ) {
    			if ( this[i] === elem ) {
    				return i;
    			}
    		}
    		return -1;
    	},
    
    	booleans = "checked|selected|async|autofocus|autoplay|controls|defer|disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped",
    
    	// Regular expressions
    
    	// http://www.w3.org/TR/css3-selectors/#whitespace
    	whitespace = "[\\x20\\t\\r\\n\\f]",
    
    	// http://www.w3.org/TR/CSS21/syndata.html#value-def-identifier
    	identifier = "(?:\\\\.|[\\w-]|[^\\x00-\\xa0])+",
    
    	// Attribute selectors: http://www.w3.org/TR/selectors/#attribute-selectors
    	attributes = "\\[" + whitespace + "*(" + identifier + ")(?:" + whitespace +
    		// Operator (capture 2)
    		"*([*^$|!~]?=)" + whitespace +
    		// "Attribute values must be CSS identifiers [capture 5] or strings [capture 3 or capture 4]"
    		"*(?:'((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\"|(" + identifier + "))|)" + whitespace +
    		"*\\]",
    
    	pseudos = ":(" + identifier + ")(?:\\((" +
    		// To reduce the number of selectors needing tokenize in the preFilter, prefer arguments:
    		// 1. quoted (capture 3; capture 4 or capture 5)
    		"('((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\")|" +
    		// 2. simple (capture 6)
    		"((?:\\\\.|[^\\\\()[\\]]|" + attributes + ")*)|" +
    		// 3. anything else (capture 2)
    		".*" +
    		")\\)|)",
    
    	// Leading and non-escaped trailing whitespace, capturing some non-whitespace characters preceding the latter
    	rtrim = new RegExp( "^" + whitespace + "+|((?:^|[^\\\\])(?:\\\\.)*)" + whitespace + "+$", "g" ),
    
    	rcomma = new RegExp( "^" + whitespace + "*," + whitespace + "*" ),
    	rcombinators = new RegExp( "^" + whitespace + "*([>+~]|" + whitespace + ")" + whitespace + "*" ),
    
    	rattributeQuotes = new RegExp( "=" + whitespace + "*([^\\]'\"]*?)" + whitespace + "*\\]", "g" ),
    
    	rpseudo = new RegExp( pseudos ),
    	ridentifier = new RegExp( "^" + identifier + "$" ),
    
    	matchExpr = {
    		"ID": new RegExp( "^#(" + identifier + ")" ),
    		"CLASS": new RegExp( "^\\.(" + identifier + ")" ),
    		"TAG": new RegExp( "^(" + identifier + "|[*])" ),
    		"ATTR": new RegExp( "^" + attributes ),
    		"PSEUDO": new RegExp( "^" + pseudos ),
    		"CHILD": new RegExp( "^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(" + whitespace +
    			"*(even|odd|(([+-]|)(\\d*)n|)" + whitespace + "*(?:([+-]|)" + whitespace +
    			"*(\\d+)|))" + whitespace + "*\\)|)", "i" ),
    		"bool": new RegExp( "^(?:" + booleans + ")$", "i" ),
    		// For use in libraries implementing .is()
    		// We use this for POS matching in `select`
    		"needsContext": new RegExp( "^" + whitespace + "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(" +
    			whitespace + "*((?:-\\d)?\\d*)" + whitespace + "*\\)|)(?=[^-]|$)", "i" )
    	},
    
    	rinputs = /^(?:input|select|textarea|button)$/i,
    	rheader = /^h\d$/i,
    
    	rnative = /^[^{]+\{\s*\[native \w/,
    
    	// Easily-parseable/retrievable ID or TAG or CLASS selectors
    	rquickExpr = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/,
    
    	rsibling = /[+~]/,
    	rescape = /'|\\/g,
    
    	// CSS escapes http://www.w3.org/TR/CSS21/syndata.html#escaped-characters
    	runescape = new RegExp( "\\\\([\\da-f]{1,6}" + whitespace + "?|(" + whitespace + ")|.)", "ig" ),
    	funescape = function( _, escaped, escapedWhitespace ) {
    		var high = "0x" + escaped - 0x10000;
    		// NaN means non-codepoint
    		// Support: Firefox<24
    		// Workaround erroneous numeric interpretation of +"0x"
    		return high !== high || escapedWhitespace ?
    			escaped :
    			high < 0 ?
    				// BMP codepoint
    				String.fromCharCode( high + 0x10000 ) :
    				// Supplemental Plane codepoint (surrogate pair)
    				String.fromCharCode( high >> 10 | 0xD800, high & 0x3FF | 0xDC00 );
    	};
    
    // Optimize for push.apply( _, NodeList )
    try {
    	push.apply(
    		(arr = slice.call( preferredDoc.childNodes )),
    		preferredDoc.childNodes
    	);
    	// Support: Android<4.0
    	// Detect silently failing push.apply
    	arr[ preferredDoc.childNodes.length ].nodeType;
    } catch ( e ) {
    	push = { apply: arr.length ?
    
    		// Leverage slice if possible
    		function( target, els ) {
    			push_native.apply( target, slice.call(els) );
    		} :
    
    		// Support: IE<9
    		// Otherwise append directly
    		function( target, els ) {
    			var j = target.length,
    				i = 0;
    			// Can't trust NodeList.length
    			while ( (target[j++] = els[i++]) ) {}
    			target.length = j - 1;
    		}
    	};
    }
    
    function Sizzle( selector, context, results, seed ) {
    	var match, elem, m, nodeType,
    		// QSA vars
    		i, groups, old, nid, newContext, newSelector;
    
    	if ( ( context ? context.ownerDocument || context : preferredDoc ) !== document ) {
    		setDocument( context );
    	}
    
    	context = context || document;
    	results = results || [];
    
    	if ( !selector || typeof selector !== "string" ) {
    		return results;
    	}
    
    	if ( (nodeType = context.nodeType) !== 1 && nodeType !== 9 ) {
    		return [];
    	}
    
    	if ( documentIsHTML && !seed ) {
    
    		// Shortcuts
    		if ( (match = rquickExpr.exec( selector )) ) {
    			// Speed-up: Sizzle("#ID")
    			if ( (m = match[1]) ) {
    				if ( nodeType === 9 ) {
    					elem = context.getElementById( m );
    					// Check parentNode to catch when Blackberry 4.6 returns
    					// nodes that are no longer in the document (jQuery #6963)
    					if ( elem && elem.parentNode ) {
    						// Handle the case where IE, Opera, and Webkit return items
    						// by name instead of ID
    						if ( elem.id === m ) {
    							results.push( elem );
    							return results;
    						}
    					} else {
    						return results;
    					}
    				} else {
    					// Context is not a document
    					if ( context.ownerDocument && (elem = context.ownerDocument.getElementById( m )) &&
    						contains( context, elem ) && elem.id === m ) {
    						results.push( elem );
    						return results;
    					}
    				}
    
    			// Speed-up: Sizzle("TAG")
    			} else if ( match[2] ) {
    				push.apply( results, context.getElementsByTagName( selector ) );
    				return results;
    
    			// Speed-up: Sizzle(".CLASS")
    			} else if ( (m = match[3]) && support.getElementsByClassName ) {
    				push.apply( results, context.getElementsByClassName( m ) );
    				return results;
    			}
    		}
    
    		// QSA path
    		if ( support.qsa && (!rbuggyQSA || !rbuggyQSA.test( selector )) ) {
    			nid = old = expando;
    			newContext = context;
    			newSelector = nodeType === 9 && selector;
    
    			// qSA works strangely on Element-rooted queries
    			// We can work around this by specifying an extra ID on the root
    			// and working up from there (Thanks to Andrew Dupont for the technique)
    			// IE 8 doesn't work on object elements
    			if ( nodeType === 1 && context.nodeName.toLowerCase() !== "object" ) {
    				groups = tokenize( selector );
    
    				if ( (old = context.getAttribute("id")) ) {
    					nid = old.replace( rescape, "\\$&" );
    				} else {
    					context.setAttribute( "id", nid );
    				}
    				nid = "[id='" + nid + "'] ";
    
    				i = groups.length;
    				while ( i-- ) {
    					groups[i] = nid + toSelector( groups[i] );
    				}
    				newContext = rsibling.test( selector ) && testContext( context.parentNode ) || context;
    				newSelector = groups.join(",");
    			}
    
    			if ( newSelector ) {
    				try {
    					push.apply( results,
    						newContext.querySelectorAll( newSelector )
    					);
    					return results;
    				} catch(qsaError) {
    				} finally {
    					if ( !old ) {
    						context.removeAttribute("id");
    					}
    				}
    			}
    		}
    	}
    
    	// All others
    	return select( selector.replace( rtrim, "$1" ), context, results, seed );
    }
    
    /**
     * Create key-value caches of limited size
     * @returns {Function(string, Object)} Returns the Object data after storing it on itself with
     *	property name the (space-suffixed) string and (if the cache is larger than Expr.cacheLength)
     *	deleting the oldest entry
     */
    function createCache() {
    	var keys = [];
    
    	function cache( key, value ) {
    		// Use (key + " ") to avoid collision with native prototype properties (see Issue #157)
    		if ( keys.push( key + " " ) > Expr.cacheLength ) {
    			// Only keep the most recent entries
    			delete cache[ keys.shift() ];
    		}
    		return (cache[ key + " " ] = value);
    	}
    	return cache;
    }
    
    /**
     * Mark a function for special use by Sizzle
     * @param {Function} fn The function to mark
     */
    function markFunction( fn ) {
    	fn[ expando ] = true;
    	return fn;
    }
    
    /**
     * Support testing using an element
     * @param {Function} fn Passed the created div and expects a boolean result
     */
    function assert( fn ) {
    	var div = document.createElement("div");
    
    	try {
    		return !!fn( div );
    	} catch (e) {
    		return false;
    	} finally {
    		// Remove from its parent by default
    		if ( div.parentNode ) {
    			div.parentNode.removeChild( div );
    		}
    		// release memory in IE
    		div = null;
    	}
    }
    
    /**
     * Adds the same handler for all of the specified attrs
     * @param {String} attrs Pipe-separated list of attributes
     * @param {Function} handler The method that will be applied
     */
    function addHandle( attrs, handler ) {
    	var arr = attrs.split("|"),
    		i = attrs.length;
    
    	while ( i-- ) {
    		Expr.attrHandle[ arr[i] ] = handler;
    	}
    }
    
    /**
     * Checks document order of two siblings
     * @param {Element} a
     * @param {Element} b
     * @returns {Number} Returns less than 0 if a precedes b, greater than 0 if a follows b
     */
    function siblingCheck( a, b ) {
    	var cur = b && a,
    		diff = cur && a.nodeType === 1 && b.nodeType === 1 &&
    			( ~b.sourceIndex || MAX_NEGATIVE ) -
    			( ~a.sourceIndex || MAX_NEGATIVE );
    
    	// Use IE sourceIndex if available on both nodes
    	if ( diff ) {
    		return diff;
    	}
    
    	// Check if b follows a
    	if ( cur ) {
    		while ( (cur = cur.nextSibling) ) {
    			if ( cur === b ) {
    				return -1;
    			}
    		}
    	}
    
    	return a ? 1 : -1;
    }
    
    /**
     * Returns a function to use in pseudos for input types
     * @param {String} type
     */
    function createInputPseudo( type ) {
    	return function( elem ) {
    		var name = elem.nodeName.toLowerCase();
    		return name === "input" && elem.type === type;
    	};
    }
    
    /**
     * Returns a function to use in pseudos for buttons
     * @param {String} type
     */
    function createButtonPseudo( type ) {
    	return function( elem ) {
    		var name = elem.nodeName.toLowerCase();
    		return (name === "input" || name === "button") && elem.type === type;
    	};
    }
    
    /**
     * Returns a function to use in pseudos for positionals
     * @param {Function} fn
     */
    function createPositionalPseudo( fn ) {
    	return markFunction(function( argument ) {
    		argument = +argument;
    		return markFunction(function( seed, matches ) {
    			var j,
    				matchIndexes = fn( [], seed.length, argument ),
    				i = matchIndexes.length;
    
    			// Match elements found at the specified indexes
    			while ( i-- ) {
    				if ( seed[ (j = matchIndexes[i]) ] ) {
    					seed[j] = !(matches[j] = seed[j]);
    				}
    			}
    		});
    	});
    }
    
    /**
     * Checks a node for validity as a Sizzle context
     * @param {Element|Object=} context
     * @returns {Element|Object|Boolean} The input node if acceptable, otherwise a falsy value
     */
    function testContext( context ) {
    	return context && typeof context.getElementsByTagName !== strundefined && context;
    }
    
    // Expose support vars for convenience
    support = Sizzle.support = {};
    
    /**
     * Detects XML nodes
     * @param {Element|Object} elem An element or a document
     * @returns {Boolean} True iff elem is a non-HTML XML node
     */
    isXML = Sizzle.isXML = function( elem ) {
    	// documentElement is verified for cases where it doesn't yet exist
    	// (such as loading iframes in IE - #4833)
    	var documentElement = elem && (elem.ownerDocument || elem).documentElement;
    	return documentElement ? documentElement.nodeName !== "HTML" : false;
    };
    
    /**
     * Sets document-related variables once based on the current document
     * @param {Element|Object} [doc] An element or document object to use to set the document
     * @returns {Object} Returns the current document
     */
    setDocument = Sizzle.setDocument = function( node ) {
    	var hasCompare,
    		doc = node ? node.ownerDocument || node : preferredDoc,
    		parent = doc.defaultView;
    
    	// If no document and documentElement is available, return
    	if ( doc === document || doc.nodeType !== 9 || !doc.documentElement ) {
    		return document;
    	}
    
    	// Set our document
    	document = doc;
    	docElem = doc.documentElement;
    
    	// Support tests
    	documentIsHTML = !isXML( doc );
    
    	// Support: IE>8
    	// If iframe document is assigned to "document" variable and if iframe has been reloaded,
    	// IE will throw "permission denied" error when accessing "document" variable, see jQuery #13936
    	// IE6-8 do not support the defaultView property so parent will be undefined
    	if ( parent && parent !== parent.top ) {
    		// IE11 does not have attachEvent, so all must suffer
    		if ( parent.addEventListener ) {
    			parent.addEventListener( "unload", function() {
    				setDocument();
    			}, false );
    		} else if ( parent.attachEvent ) {
    			parent.attachEvent( "onunload", function() {
    				setDocument();
    			});
    		}
    	}
    
    	/* Attributes
    	---------------------------------------------------------------------- */
    
    	// Support: IE<8
    	// Verify that getAttribute really returns attributes and not properties (excepting IE8 booleans)
    	support.attributes = assert(function( div ) {
    		div.className = "i";
    		return !div.getAttribute("className");
    	});
    
    	/* getElement(s)By*
    	---------------------------------------------------------------------- */
    
    	// Check if getElementsByTagName("*") returns only elements
    	support.getElementsByTagName = assert(function( div ) {
    		div.appendChild( doc.createComment("") );
    		return !div.getElementsByTagName("*").length;
    	});
    
    	// Support: IE<9
    	support.getElementsByClassName = rnative.test( doc.getElementsByClassName );
    
    	// Support: IE<10
    	// Check if getElementById returns elements by name
    	// The broken getElementById methods don't pick up programatically-set names,
    	// so use a roundabout getElementsByName test
    	support.getById = assert(function( div ) {
    		docElem.appendChild( div ).id = expando;
    		return !doc.getElementsByName || !doc.getElementsByName( expando ).length;
    	});
    
    	// ID find and filter
    	if ( support.getById ) {
    		Expr.find["ID"] = function( id, context ) {
    			if ( typeof context.getElementById !== strundefined && documentIsHTML ) {
    				var m = context.getElementById( id );
    				// Check parentNode to catch when Blackberry 4.6 returns
    				// nodes that are no longer in the document #6963
    				return m && m.parentNode ? [ m ] : [];
    			}
    		};
    		Expr.filter["ID"] = function( id ) {
    			var attrId = id.replace( runescape, funescape );
    			return function( elem ) {
    				return elem.getAttribute("id") === attrId;
    			};
    		};
    	} else {
    		// Support: IE6/7
    		// getElementById is not reliable as a find shortcut
    		delete Expr.find["ID"];
    
    		Expr.filter["ID"] =  function( id ) {
    			var attrId = id.replace( runescape, funescape );
    			return function( elem ) {
    				var node = typeof elem.getAttributeNode !== strundefined && elem.getAttributeNode("id");
    				return node && node.value === attrId;
    			};
    		};
    	}
    
    	// Tag
    	Expr.find["TAG"] = support.getElementsByTagName ?
    		function( tag, context ) {
    			if ( typeof context.getElementsByTagName !== strundefined ) {
    				return context.getElementsByTagName( tag );
    			}
    		} :
    		function( tag, context ) {
    			var elem,
    				tmp = [],
    				i = 0,
    				results = context.getElementsByTagName( tag );
    
    			// Filter out possible comments
    			if ( tag === "*" ) {
    				while ( (elem = results[i++]) ) {
    					if ( elem.nodeType === 1 ) {
    						tmp.push( elem );
    					}
    				}
    
    				return tmp;
    			}
    			return results;
    		};
    
    	// Class
    	Expr.find["CLASS"] = support.getElementsByClassName && function( className, context ) {
    		if ( documentIsHTML ) {
    			return context.getElementsByClassName( className );
    		}
    	};
    
    	/* QSA/matchesSelector
    	---------------------------------------------------------------------- */
    
    	// QSA and matchesSelector support
    
    	// matchesSelector(:active) reports false when true (IE9/Opera 11.5)
    	rbuggyMatches = [];
    
    	// qSa(:focus) reports false when true (Chrome 21)
    	// We allow this because of a bug in IE8/9 that throws an error
    	// whenever `document.activeElement` is accessed on an iframe
    	// So, we allow :focus to pass through QSA all the time to avoid the IE error
    	// See http://bugs.jquery.com/ticket/13378
    	rbuggyQSA = [];
    
    	if ( (support.qsa = rnative.test( doc.querySelectorAll )) ) {
    		// Build QSA regex
    		// Regex strategy adopted from Diego Perini
    		assert(function( div ) {
    			// Select is set to empty string on purpose
    			// This is to test IE's treatment of not explicitly
    			// setting a boolean content attribute,
    			// since its presence should be enough
    			// http://bugs.jquery.com/ticket/12359
    			div.innerHTML = "<select msallowcapture=''><option selected=''></option></select>";
    
    			// Support: IE8, Opera 11-12.16
    			// Nothing should be selected when empty strings follow ^= or $= or *=
    			// The test attribute must be unknown in Opera but "safe" for WinRT
    			// http://msdn.microsoft.com/en-us/library/ie/hh465388.aspx#attribute_section
    			if ( div.querySelectorAll("[msallowcapture^='']").length ) {
    				rbuggyQSA.push( "[*^$]=" + whitespace + "*(?:''|\"\")" );
    			}
    
    			// Support: IE8
    			// Boolean attributes and "value" are not treated correctly
    			if ( !div.querySelectorAll("[selected]").length ) {
    				rbuggyQSA.push( "\\[" + whitespace + "*(?:value|" + booleans + ")" );
    			}
    
    			// Webkit/Opera - :checked should return selected option elements
    			// http://www.w3.org/TR/2011/REC-css3-selectors-20110929/#checked
    			// IE8 throws error here and will not see later tests
    			if ( !div.querySelectorAll(":checked").length ) {
    				rbuggyQSA.push(":checked");
    			}
    		});
    
    		assert(function( div ) {
    			// Support: Windows 8 Native Apps
    			// The type and name attributes are restricted during .innerHTML assignment
    			var input = doc.createElement("input");
    			input.setAttribute( "type", "hidden" );
    			div.appendChild( input ).setAttribute( "name", "D" );
    
    			// Support: IE8
    			// Enforce case-sensitivity of name attribute
    			if ( div.querySelectorAll("[name=d]").length ) {
    				rbuggyQSA.push( "name" + whitespace + "*[*^$|!~]?=" );
    			}
    
    			// FF 3.5 - :enabled/:disabled and hidden elements (hidden elements are still enabled)
    			// IE8 throws error here and will not see later tests
    			if ( !div.querySelectorAll(":enabled").length ) {
    				rbuggyQSA.push( ":enabled", ":disabled" );
    			}
    
    			// Opera 10-11 does not throw on post-comma invalid pseudos
    			div.querySelectorAll("*,:x");
    			rbuggyQSA.push(",.*:");
    		});
    	}
    
    	if ( (support.matchesSelector = rnative.test( (matches = docElem.matches ||
    		docElem.webkitMatchesSelector ||
    		docElem.mozMatchesSelector ||
    		docElem.oMatchesSelector ||
    		docElem.msMatchesSelector) )) ) {
    
    		assert(function( div ) {
    			// Check to see if it's possible to do matchesSelector
    			// on a disconnected node (IE 9)
    			support.disconnectedMatch = matches.call( div, "div" );
    
    			// This should fail with an exception
    			// Gecko does not error, returns false instead
    			matches.call( div, "[s!='']:x" );
    			rbuggyMatches.push( "!=", pseudos );
    		});
    	}
    
    	rbuggyQSA = rbuggyQSA.length && new RegExp( rbuggyQSA.join("|") );
    	rbuggyMatches = rbuggyMatches.length && new RegExp( rbuggyMatches.join("|") );
    
    	/* Contains
    	---------------------------------------------------------------------- */
    	hasCompare = rnative.test( docElem.compareDocumentPosition );
    
    	// Element contains another
    	// Purposefully does not implement inclusive descendent
    	// As in, an element does not contain itself
    	contains = hasCompare || rnative.test( docElem.contains ) ?
    		function( a, b ) {
    			var adown = a.nodeType === 9 ? a.documentElement : a,
    				bup = b && b.parentNode;
    			return a === bup || !!( bup && bup.nodeType === 1 && (
    				adown.contains ?
    					adown.contains( bup ) :
    					a.compareDocumentPosition && a.compareDocumentPosition( bup ) & 16
    			));
    		} :
    		function( a, b ) {
    			if ( b ) {
    				while ( (b = b.parentNode) ) {
    					if ( b === a ) {
    						return true;
    					}
    				}
    			}
    			return false;
    		};
    
    	/* Sorting
    	---------------------------------------------------------------------- */
    
    	// Document order sorting
    	sortOrder = hasCompare ?
    	function( a, b ) {
    
    		// Flag for duplicate removal
    		if ( a === b ) {
    			hasDuplicate = true;
    			return 0;
    		}
    
    		// Sort on method existence if only one input has compareDocumentPosition
    		var compare = !a.compareDocumentPosition - !b.compareDocumentPosition;
    		if ( compare ) {
    			return compare;
    		}
    
    		// Calculate position if both inputs belong to the same document
    		compare = ( a.ownerDocument || a ) === ( b.ownerDocument || b ) ?
    			a.compareDocumentPosition( b ) :
    
    			// Otherwise we know they are disconnected
    			1;
    
    		// Disconnected nodes
    		if ( compare & 1 ||
    			(!support.sortDetached && b.compareDocumentPosition( a ) === compare) ) {
    
    			// Choose the first element that is related to our preferred document
    			if ( a === doc || a.ownerDocument === preferredDoc && contains(preferredDoc, a) ) {
    				return -1;
    			}
    			if ( b === doc || b.ownerDocument === preferredDoc && contains(preferredDoc, b) ) {
    				return 1;
    			}
    
    			// Maintain original order
    			return sortInput ?
    				( indexOf.call( sortInput, a ) - indexOf.call( sortInput, b ) ) :
    				0;
    		}
    
    		return compare & 4 ? -1 : 1;
    	} :
    	function( a, b ) {
    		// Exit early if the nodes are identical
    		if ( a === b ) {
    			hasDuplicate = true;
    			return 0;
    		}
    
    		var cur,
    			i = 0,
    			aup = a.parentNode,
    			bup = b.parentNode,
    			ap = [ a ],
    			bp = [ b ];
    
    		// Parentless nodes are either documents or disconnected
    		if ( !aup || !bup ) {
    			return a === doc ? -1 :
    				b === doc ? 1 :
    				aup ? -1 :
    				bup ? 1 :
    				sortInput ?
    				( indexOf.call( sortInput, a ) - indexOf.call( sortInput, b ) ) :
    				0;
    
    		// If the nodes are siblings, we can do a quick check
    		} else if ( aup === bup ) {
    			return siblingCheck( a, b );
    		}
    
    		// Otherwise we need full lists of their ancestors for comparison
    		cur = a;
    		while ( (cur = cur.parentNode) ) {
    			ap.unshift( cur );
    		}
    		cur = b;
    		while ( (cur = cur.parentNode) ) {
    			bp.unshift( cur );
    		}
    
    		// Walk down the tree looking for a discrepancy
    		while ( ap[i] === bp[i] ) {
    			i++;
    		}
    
    		return i ?
    			// Do a sibling check if the nodes have a common ancestor
    			siblingCheck( ap[i], bp[i] ) :
    
    			// Otherwise nodes in our document sort first
    			ap[i] === preferredDoc ? -1 :
    			bp[i] === preferredDoc ? 1 :
    			0;
    	};
    
    	return doc;
    };
    
    Sizzle.matches = function( expr, elements ) {
    	return Sizzle( expr, null, null, elements );
    };
    
    Sizzle.matchesSelector = function( elem, expr ) {
    	// Set document vars if needed
    	if ( ( elem.ownerDocument || elem ) !== document ) {
    		setDocument( elem );
    	}
    
    	// Make sure that attribute selectors are quoted
    	expr = expr.replace( rattributeQuotes, "='$1']" );
    
    	if ( support.matchesSelector && documentIsHTML &&
    		( !rbuggyMatches || !rbuggyMatches.test( expr ) ) &&
    		( !rbuggyQSA     || !rbuggyQSA.test( expr ) ) ) {
    
    		try {
    			var ret = matches.call( elem, expr );
    
    			// IE 9's matchesSelector returns false on disconnected nodes
    			if ( ret || support.disconnectedMatch ||
    					// As well, disconnected nodes are said to be in a document
    					// fragment in IE 9
    					elem.document && elem.document.nodeType !== 11 ) {
    				return ret;
    			}
    		} catch(e) {}
    	}
    
    	return Sizzle( expr, document, null, [ elem ] ).length > 0;
    };
    
    Sizzle.contains = function( context, elem ) {
    	// Set document vars if needed
    	if ( ( context.ownerDocument || context ) !== document ) {
    		setDocument( context );
    	}
    	return contains( context, elem );
    };
    
    Sizzle.attr = function( elem, name ) {
    	// Set document vars if needed
    	if ( ( elem.ownerDocument || elem ) !== document ) {
    		setDocument( elem );
    	}
    
    	var fn = Expr.attrHandle[ name.toLowerCase() ],
    		// Don't get fooled by Object.prototype properties (jQuery #13807)
    		val = fn && hasOwn.call( Expr.attrHandle, name.toLowerCase() ) ?
    			fn( elem, name, !documentIsHTML ) :
    			undefined;
    
    	return val !== undefined ?
    		val :
    		support.attributes || !documentIsHTML ?
    			elem.getAttribute( name ) :
    			(val = elem.getAttributeNode(name)) && val.specified ?
    				val.value :
    				null;
    };
    
    Sizzle.error = function( msg ) {
    	throw new Error( "Syntax error, unrecognized expression: " + msg );
    };
    
    /**
     * Document sorting and removing duplicates
     * @param {ArrayLike} results
     */
    Sizzle.uniqueSort = function( results ) {
    	var elem,
    		duplicates = [],
    		j = 0,
    		i = 0;
    
    	// Unless we *know* we can detect duplicates, assume their presence
    	hasDuplicate = !support.detectDuplicates;
    	sortInput = !support.sortStable && results.slice( 0 );
    	results.sort( sortOrder );
    
    	if ( hasDuplicate ) {
    		while ( (elem = results[i++]) ) {
    			if ( elem === results[ i ] ) {
    				j = duplicates.push( i );
    			}
    		}
    		while ( j-- ) {
    			results.splice( duplicates[ j ], 1 );
    		}
    	}
    
    	// Clear input after sorting to release objects
    	// See https://github.com/jquery/sizzle/pull/225
    	sortInput = null;
    
    	return results;
    };
    
    /**
     * Utility function for retrieving the text value of an array of DOM nodes
     * @param {Array|Element} elem
     */
    getText = Sizzle.getText = function( elem ) {
    	var node,
    		ret = "",
    		i = 0,
    		nodeType = elem.nodeType;
    
    	if ( !nodeType ) {
    		// If no nodeType, this is expected to be an array
    		while ( (node = elem[i++]) ) {
    			// Do not traverse comment nodes
    			ret += getText( node );
    		}
    	} else if ( nodeType === 1 || nodeType === 9 || nodeType === 11 ) {
    		// Use textContent for elements
    		// innerText usage removed for consistency of new lines (jQuery #11153)
    		if ( typeof elem.textContent === "string" ) {
    			return elem.textContent;
    		} else {
    			// Traverse its children
    			for ( elem = elem.firstChild; elem; elem = elem.nextSibling ) {
    				ret += getText( elem );
    			}
    		}
    	} else if ( nodeType === 3 || nodeType === 4 ) {
    		return elem.nodeValue;
    	}
    	// Do not include comment or processing instruction nodes
    
    	return ret;
    };
    
    Expr = Sizzle.selectors = {
    
    	// Can be adjusted by the user
    	cacheLength: 50,
    
    	createPseudo: markFunction,
    
    	match: matchExpr,
    
    	attrHandle: {},
    
    	find: {},
    
    	relative: {
    		">": { dir: "parentNode", first: true },
    		" ": { dir: "parentNode" },
    		"+": { dir: "previousSibling", first: true },
    		"~": { dir: "previousSibling" }
    	},
    
    	preFilter: {
    		"ATTR": function( match ) {
    			match[1] = match[1].replace( runescape, funescape );
    
    			// Move the given value to match[3] whether quoted or unquoted
    			match[3] = ( match[3] || match[4] || match[5] || "" ).replace( runescape, funescape );
    
    			if ( match[2] === "~=" ) {
    				match[3] = " " + match[3] + " ";
    			}
    
    			return match.slice( 0, 4 );
    		},
    
    		"CHILD": function( match ) {
    			/* matches from matchExpr["CHILD"]
    				1 type (only|nth|...)
    				2 what (child|of-type)
    				3 argument (even|odd|\d*|\d*n([+-]\d+)?|...)
    				4 xn-component of xn+y argument ([+-]?\d*n|)
    				5 sign of xn-component
    				6 x of xn-component
    				7 sign of y-component
    				8 y of y-component
    			*/
    			match[1] = match[1].toLowerCase();
    
    			if ( match[1].slice( 0, 3 ) === "nth" ) {
    				// nth-* requires argument
    				if ( !match[3] ) {
    					Sizzle.error( match[0] );
    				}
    
    				// numeric x and y parameters for Expr.filter.CHILD
    				// remember that false/true cast respectively to 0/1
    				match[4] = +( match[4] ? match[5] + (match[6] || 1) : 2 * ( match[3] === "even" || match[3] === "odd" ) );
    				match[5] = +( ( match[7] + match[8] ) || match[3] === "odd" );
    
    			// other types prohibit arguments
    			} else if ( match[3] ) {
    				Sizzle.error( match[0] );
    			}
    
    			return match;
    		},
    
    		"PSEUDO": function( match ) {
    			var excess,
    				unquoted = !match[6] && match[2];
    
    			if ( matchExpr["CHILD"].test( match[0] ) ) {
    				return null;
    			}
    
    			// Accept quoted arguments as-is
    			if ( match[3] ) {
    				match[2] = match[4] || match[5] || "";
    
    			// Strip excess characters from unquoted arguments
    			} else if ( unquoted && rpseudo.test( unquoted ) &&
    				// Get excess from tokenize (recursively)
    				(excess = tokenize( unquoted, true )) &&
    				// advance to the next closing parenthesis
    				(excess = unquoted.indexOf( ")", unquoted.length - excess ) - unquoted.length) ) {
    
    				// excess is a negative index
    				match[0] = match[0].slice( 0, excess );
    				match[2] = unquoted.slice( 0, excess );
    			}
    
    			// Return only captures needed by the pseudo filter method (type and argument)
    			return match.slice( 0, 3 );
    		}
    	},
    
    	filter: {
    
    		"TAG": function( nodeNameSelector ) {
    			var nodeName = nodeNameSelector.replace( runescape, funescape ).toLowerCase();
    			return nodeNameSelector === "*" ?
    				function() { return true; } :
    				function( elem ) {
    					return elem.nodeName && elem.nodeName.toLowerCase() === nodeName;
    				};
    		},
    
    		"CLASS": function( className ) {
    			var pattern = classCache[ className + " " ];
    
    			return pattern ||
    				(pattern = new RegExp( "(^|" + whitespace + ")" + className + "(" + whitespace + "|$)" )) &&
    				classCache( className, function( elem ) {
    					return pattern.test( typeof elem.className === "string" && elem.className || typeof elem.getAttribute !== strundefined && elem.getAttribute("class") || "" );
    				});
    		},
    
    		"ATTR": function( name, operator, check ) {
    			return function( elem ) {
    				var result = Sizzle.attr( elem, name );
    
    				if ( result == null ) {
    					return operator === "!=";
    				}
    				if ( !operator ) {
    					return true;
    				}
    
    				result += "";
    
    				return operator === "=" ? result === check :
    					operator === "!=" ? result !== check :
    					operator === "^=" ? check && result.indexOf( check ) === 0 :
    					operator === "*=" ? check && result.indexOf( check ) > -1 :
    					operator === "$=" ? check && result.slice( -check.length ) === check :
    					operator === "~=" ? ( " " + result + " " ).indexOf( check ) > -1 :
    					operator === "|=" ? result === check || result.slice( 0, check.length + 1 ) === check + "-" :
    					false;
    			};
    		},
    
    		"CHILD": function( type, what, argument, first, last ) {
    			var simple = type.slice( 0, 3 ) !== "nth",
    				forward = type.slice( -4 ) !== "last",
    				ofType = what === "of-type";
    
    			return first === 1 && last === 0 ?
    
    				// Shortcut for :nth-*(n)
    				function( elem ) {
    					return !!elem.parentNode;
    				} :
    
    				function( elem, context, xml ) {
    					var cache, outerCache, node, diff, nodeIndex, start,
    						dir = simple !== forward ? "nextSibling" : "previousSibling",
    						parent = elem.parentNode,
    						name = ofType && elem.nodeName.toLowerCase(),
    						useCache = !xml && !ofType;
    
    					if ( parent ) {
    
    						// :(first|last|only)-(child|of-type)
    						if ( simple ) {
    							while ( dir ) {
    								node = elem;
    								while ( (node = node[ dir ]) ) {
    									if ( ofType ? node.nodeName.toLowerCase() === name : node.nodeType === 1 ) {
    										return false;
    									}
    								}
    								// Reverse direction for :only-* (if we haven't yet done so)
    								start = dir = type === "only" && !start && "nextSibling";
    							}
    							return true;
    						}
    
    						start = [ forward ? parent.firstChild : parent.lastChild ];
    
    						// non-xml :nth-child(...) stores cache data on `parent`
    						if ( forward && useCache ) {
    							// Seek `elem` from a previously-cached index
    							outerCache = parent[ expando ] || (parent[ expando ] = {});
    							cache = outerCache[ type ] || [];
    							nodeIndex = cache[0] === dirruns && cache[1];
    							diff = cache[0] === dirruns && cache[2];
    							node = nodeIndex && parent.childNodes[ nodeIndex ];
    
    							while ( (node = ++nodeIndex && node && node[ dir ] ||
    
    								// Fallback to seeking `elem` from the start
    								(diff = nodeIndex = 0) || start.pop()) ) {
    
    								// When found, cache indexes on `parent` and break
    								if ( node.nodeType === 1 && ++diff && node === elem ) {
    									outerCache[ type ] = [ dirruns, nodeIndex, diff ];
    									break;
    								}
    							}
    
    						// Use previously-cached element index if available
    						} else if ( useCache && (cache = (elem[ expando ] || (elem[ expando ] = {}))[ type ]) && cache[0] === dirruns ) {
    							diff = cache[1];
    
    						// xml :nth-child(...) or :nth-last-child(...) or :nth(-last)?-of-type(...)
    						} else {
    							// Use the same loop as above to seek `elem` from the start
    							while ( (node = ++nodeIndex && node && node[ dir ] ||
    								(diff = nodeIndex = 0) || start.pop()) ) {
    
    								if ( ( ofType ? node.nodeName.toLowerCase() === name : node.nodeType === 1 ) && ++diff ) {
    									// Cache the index of each encountered element
    									if ( useCache ) {
    										(node[ expando ] || (node[ expando ] = {}))[ type ] = [ dirruns, diff ];
    									}
    
    									if ( node === elem ) {
    										break;
    									}
    								}
    							}
    						}
    
    						// Incorporate the offset, then check against cycle size
    						diff -= last;
    						return diff === first || ( diff % first === 0 && diff / first >= 0 );
    					}
    				};
    		},
    
    		"PSEUDO": function( pseudo, argument ) {
    			// pseudo-class names are case-insensitive
    			// http://www.w3.org/TR/selectors/#pseudo-classes
    			// Prioritize by case sensitivity in case custom pseudos are added with uppercase letters
    			// Remember that setFilters inherits from pseudos
    			var args,
    				fn = Expr.pseudos[ pseudo ] || Expr.setFilters[ pseudo.toLowerCase() ] ||
    					Sizzle.error( "unsupported pseudo: " + pseudo );
    
    			// The user may use createPseudo to indicate that
    			// arguments are needed to create the filter function
    			// just as Sizzle does
    			if ( fn[ expando ] ) {
    				return fn( argument );
    			}
    
    			// But maintain support for old signatures
    			if ( fn.length > 1 ) {
    				args = [ pseudo, pseudo, "", argument ];
    				return Expr.setFilters.hasOwnProperty( pseudo.toLowerCase() ) ?
    					markFunction(function( seed, matches ) {
    						var idx,
    							matched = fn( seed, argument ),
    							i = matched.length;
    						while ( i-- ) {
    							idx = indexOf.call( seed, matched[i] );
    							seed[ idx ] = !( matches[ idx ] = matched[i] );
    						}
    					}) :
    					function( elem ) {
    						return fn( elem, 0, args );
    					};
    			}
    
    			return fn;
    		}
    	},
    
    	pseudos: {
    		// Potentially complex pseudos
    		"not": markFunction(function( selector ) {
    			// Trim the selector passed to compile
    			// to avoid treating leading and trailing
    			// spaces as combinators
    			var input = [],
    				results = [],
    				matcher = compile( selector.replace( rtrim, "$1" ) );
    
    			return matcher[ expando ] ?
    				markFunction(function( seed, matches, context, xml ) {
    					var elem,
    						unmatched = matcher( seed, null, xml, [] ),
    						i = seed.length;
    
    					// Match elements unmatched by `matcher`
    					while ( i-- ) {
    						if ( (elem = unmatched[i]) ) {
    							seed[i] = !(matches[i] = elem);
    						}
    					}
    				}) :
    				function( elem, context, xml ) {
    					input[0] = elem;
    					matcher( input, null, xml, results );
    					return !results.pop();
    				};
    		}),
    
    		"has": markFunction(function( selector ) {
    			return function( elem ) {
    				return Sizzle( selector, elem ).length > 0;
    			};
    		}),
    
    		"contains": markFunction(function( text ) {
    			text = text.replace( runescape, funescape );
    			return function( elem ) {
    				return ( elem.textContent || elem.innerText || getText( elem ) ).indexOf( text ) > -1;
    			};
    		}),
    
    		// "Whether an element is represented by a :lang() selector
    		// is based solely on the element's language value
    		// being equal to the identifier C,
    		// or beginning with the identifier C immediately followed by "-".
    		// The matching of C against the element's language value is performed case-insensitively.
    		// The identifier C does not have to be a valid language name."
    		// http://www.w3.org/TR/selectors/#lang-pseudo
    		"lang": markFunction( function( lang ) {
    			// lang value must be a valid identifier
    			if ( !ridentifier.test(lang || "") ) {
    				Sizzle.error( "unsupported lang: " + lang );
    			}
    			lang = lang.replace( runescape, funescape ).toLowerCase();
    			return function( elem ) {
    				var elemLang;
    				do {
    					if ( (elemLang = documentIsHTML ?
    						elem.lang :
    						elem.getAttribute("xml:lang") || elem.getAttribute("lang")) ) {
    
    						elemLang = elemLang.toLowerCase();
    						return elemLang === lang || elemLang.indexOf( lang + "-" ) === 0;
    					}
    				} while ( (elem = elem.parentNode) && elem.nodeType === 1 );
    				return false;
    			};
    		}),
    
    		// Miscellaneous
    		"target": function( elem ) {
    			var hash = window.location && window.location.hash;
    			return hash && hash.slice( 1 ) === elem.id;
    		},
    
    		"root": function( elem ) {
    			return elem === docElem;
    		},
    
    		"focus": function( elem ) {
    			return elem === document.activeElement && (!document.hasFocus || document.hasFocus()) && !!(elem.type || elem.href || ~elem.tabIndex);
    		},
    
    		// Boolean properties
    		"enabled": function( elem ) {
    			return elem.disabled === false;
    		},
    
    		"disabled": function( elem ) {
    			return elem.disabled === true;
    		},
    
    		"checked": function( elem ) {
    			// In CSS3, :checked should return both checked and selected elements
    			// http://www.w3.org/TR/2011/REC-css3-selectors-20110929/#checked
    			var nodeName = elem.nodeName.toLowerCase();
    			return (nodeName === "input" && !!elem.checked) || (nodeName === "option" && !!elem.selected);
    		},
    
    		"selected": function( elem ) {
    			// Accessing this property makes selected-by-default
    			// options in Safari work properly
    			if ( elem.parentNode ) {
    				elem.parentNode.selectedIndex;
    			}
    
    			return elem.selected === true;
    		},
    
    		// Contents
    		"empty": function( elem ) {
    			// http://www.w3.org/TR/selectors/#empty-pseudo
    			// :empty is negated by element (1) or content nodes (text: 3; cdata: 4; entity ref: 5),
    			//   but not by others (comment: 8; processing instruction: 7; etc.)
    			// nodeType < 6 works because attributes (2) do not appear as children
    			for ( elem = elem.firstChild; elem; elem = elem.nextSibling ) {
    				if ( elem.nodeType < 6 ) {
    					return false;
    				}
    			}
    			return true;
    		},
    
    		"parent": function( elem ) {
    			return !Expr.pseudos["empty"]( elem );
    		},
    
    		// Element/input types
    		"header": function( elem ) {
    			return rheader.test( elem.nodeName );
    		},
    
    		"input": function( elem ) {
    			return rinputs.test( elem.nodeName );
    		},
    
    		"button": function( elem ) {
    			var name = elem.nodeName.toLowerCase();
    			return name === "input" && elem.type === "button" || name === "button";
    		},
    
    		"text": function( elem ) {
    			var attr;
    			return elem.nodeName.toLowerCase() === "input" &&
    				elem.type === "text" &&
    
    				// Support: IE<8
    				// New HTML5 attribute values (e.g., "search") appear with elem.type === "text"
    				( (attr = elem.getAttribute("type")) == null || attr.toLowerCase() === "text" );
    		},
    
    		// Position-in-collection
    		"first": createPositionalPseudo(function() {
    			return [ 0 ];
    		}),
    
    		"last": createPositionalPseudo(function( matchIndexes, length ) {
    			return [ length - 1 ];
    		}),
    
    		"eq": createPositionalPseudo(function( matchIndexes, length, argument ) {
    			return [ argument < 0 ? argument + length : argument ];
    		}),
    
    		"even": createPositionalPseudo(function( matchIndexes, length ) {
    			var i = 0;
    			for ( ; i < length; i += 2 ) {
    				matchIndexes.push( i );
    			}
    			return matchIndexes;
    		}),
    
    		"odd": createPositionalPseudo(function( matchIndexes, length ) {
    			var i = 1;
    			for ( ; i < length; i += 2 ) {
    				matchIndexes.push( i );
    			}
    			return matchIndexes;
    		}),
    
    		"lt": createPositionalPseudo(function( matchIndexes, length, argument ) {
    			var i = argument < 0 ? argument + length : argument;
    			for ( ; --i >= 0; ) {
    				matchIndexes.push( i );
    			}
    			return matchIndexes;
    		}),
    
    		"gt": createPositionalPseudo(function( matchIndexes, length, argument ) {
    			var i = argument < 0 ? argument + length : argument;
    			for ( ; ++i < length; ) {
    				matchIndexes.push( i );
    			}
    			return matchIndexes;
    		})
    	}
    };
    
    Expr.pseudos["nth"] = Expr.pseudos["eq"];
    
    // Add button/input type pseudos
    for ( i in { radio: true, checkbox: true, file: true, password: true, image: true } ) {
    	Expr.pseudos[ i ] = createInputPseudo( i );
    }
    for ( i in { submit: true, reset: true } ) {
    	Expr.pseudos[ i ] = createButtonPseudo( i );
    }
    
    // Easy API for creating new setFilters
    function setFilters() {}
    setFilters.prototype = Expr.filters = Expr.pseudos;
    Expr.setFilters = new setFilters();
    
    tokenize = Sizzle.tokenize = function( selector, parseOnly ) {
    	var matched, match, tokens, type,
    		soFar, groups, preFilters,
    		cached = tokenCache[ selector + " " ];
    
    	if ( cached ) {
    		return parseOnly ? 0 : cached.slice( 0 );
    	}
    
    	soFar = selector;
    	groups = [];
    	preFilters = Expr.preFilter;
    
    	while ( soFar ) {
    
    		// Comma and first run
    		if ( !matched || (match = rcomma.exec( soFar )) ) {
    			if ( match ) {
    				// Don't consume trailing commas as valid
    				soFar = soFar.slice( match[0].length ) || soFar;
    			}
    			groups.push( (tokens = []) );
    		}
    
    		matched = false;
    
    		// Combinators
    		if ( (match = rcombinators.exec( soFar )) ) {
    			matched = match.shift();
    			tokens.push({
    				value: matched,
    				// Cast descendant combinators to space
    				type: match[0].replace( rtrim, " " )
    			});
    			soFar = soFar.slice( matched.length );
    		}
    
    		// Filters
    		for ( type in Expr.filter ) {
    			if ( (match = matchExpr[ type ].exec( soFar )) && (!preFilters[ type ] ||
    				(match = preFilters[ type ]( match ))) ) {
    				matched = match.shift();
    				tokens.push({
    					value: matched,
    					type: type,
    					matches: match
    				});
    				soFar = soFar.slice( matched.length );
    			}
    		}
    
    		if ( !matched ) {
    			break;
    		}
    	}
    
    	// Return the length of the invalid excess
    	// if we're just parsing
    	// Otherwise, throw an error or return tokens
    	return parseOnly ?
    		soFar.length :
    		soFar ?
    			Sizzle.error( selector ) :
    			// Cache the tokens
    			tokenCache( selector, groups ).slice( 0 );
    };
    
    function toSelector( tokens ) {
    	var i = 0,
    		len = tokens.length,
    		selector = "";
    	for ( ; i < len; i++ ) {
    		selector += tokens[i].value;
    	}
    	return selector;
    }
    
    function addCombinator( matcher, combinator, base ) {
    	var dir = combinator.dir,
    		checkNonElements = base && dir === "parentNode",
    		doneName = done++;
    
    	return combinator.first ?
    		// Check against closest ancestor/preceding element
    		function( elem, context, xml ) {
    			while ( (elem = elem[ dir ]) ) {
    				if ( elem.nodeType === 1 || checkNonElements ) {
    					return matcher( elem, context, xml );
    				}
    			}
    		} :
    
    		// Check against all ancestor/preceding elements
    		function( elem, context, xml ) {
    			var oldCache, outerCache,
    				newCache = [ dirruns, doneName ];
    
    			// We can't set arbitrary data on XML nodes, so they don't benefit from dir caching
    			if ( xml ) {
    				while ( (elem = elem[ dir ]) ) {
    					if ( elem.nodeType === 1 || checkNonElements ) {
    						if ( matcher( elem, context, xml ) ) {
    							return true;
    						}
    					}
    				}
    			} else {
    				while ( (elem = elem[ dir ]) ) {
    					if ( elem.nodeType === 1 || checkNonElements ) {
    						outerCache = elem[ expando ] || (elem[ expando ] = {});
    						if ( (oldCache = outerCache[ dir ]) &&
    							oldCache[ 0 ] === dirruns && oldCache[ 1 ] === doneName ) {
    
    							// Assign to newCache so results back-propagate to previous elements
    							return (newCache[ 2 ] = oldCache[ 2 ]);
    						} else {
    							// Reuse newcache so results back-propagate to previous elements
    							outerCache[ dir ] = newCache;
    
    							// A match means we're done; a fail means we have to keep checking
    							if ( (newCache[ 2 ] = matcher( elem, context, xml )) ) {
    								return true;
    							}
    						}
    					}
    				}
    			}
    		};
    }
    
    function elementMatcher( matchers ) {
    	return matchers.length > 1 ?
    		function( elem, context, xml ) {
    			var i = matchers.length;
    			while ( i-- ) {
    				if ( !matchers[i]( elem, context, xml ) ) {
    					return false;
    				}
    			}
    			return true;
    		} :
    		matchers[0];
    }
    
    function multipleContexts( selector, contexts, results ) {
    	var i = 0,
    		len = contexts.length;
    	for ( ; i < len; i++ ) {
    		Sizzle( selector, contexts[i], results );
    	}
    	return results;
    }
    
    function condense( unmatched, map, filter, context, xml ) {
    	var elem,
    		newUnmatched = [],
    		i = 0,
    		len = unmatched.length,
    		mapped = map != null;
    
    	for ( ; i < len; i++ ) {
    		if ( (elem = unmatched[i]) ) {
    			if ( !filter || filter( elem, context, xml ) ) {
    				newUnmatched.push( elem );
    				if ( mapped ) {
    					map.push( i );
    				}
    			}
    		}
    	}
    
    	return newUnmatched;
    }
    
    function setMatcher( preFilter, selector, matcher, postFilter, postFinder, postSelector ) {
    	if ( postFilter && !postFilter[ expando ] ) {
    		postFilter = setMatcher( postFilter );
    	}
    	if ( postFinder && !postFinder[ expando ] ) {
    		postFinder = setMatcher( postFinder, postSelector );
    	}
    	return markFunction(function( seed, results, context, xml ) {
    		var temp, i, elem,
    			preMap = [],
    			postMap = [],
    			preexisting = results.length,
    
    			// Get initial elements from seed or context
    			elems = seed || multipleContexts( selector || "*", context.nodeType ? [ context ] : context, [] ),
    
    			// Prefilter to get matcher input, preserving a map for seed-results synchronization
    			matcherIn = preFilter && ( seed || !selector ) ?
    				condense( elems, preMap, preFilter, context, xml ) :
    				elems,
    
    			matcherOut = matcher ?
    				// If we have a postFinder, or filtered seed, or non-seed postFilter or preexisting results,
    				postFinder || ( seed ? preFilter : preexisting || postFilter ) ?
    
    					// ...intermediate processing is necessary
    					[] :
    
    					// ...otherwise use results directly
    					results :
    				matcherIn;
    
    		// Find primary matches
    		if ( matcher ) {
    			matcher( matcherIn, matcherOut, context, xml );
    		}
    
    		// Apply postFilter
    		if ( postFilter ) {
    			temp = condense( matcherOut, postMap );
    			postFilter( temp, [], context, xml );
    
    			// Un-match failing elements by moving them back to matcherIn
    			i = temp.length;
    			while ( i-- ) {
    				if ( (elem = temp[i]) ) {
    					matcherOut[ postMap[i] ] = !(matcherIn[ postMap[i] ] = elem);
    				}
    			}
    		}
    
    		if ( seed ) {
    			if ( postFinder || preFilter ) {
    				if ( postFinder ) {
    					// Get the final matcherOut by condensing this intermediate into postFinder contexts
    					temp = [];
    					i = matcherOut.length;
    					while ( i-- ) {
    						if ( (elem = matcherOut[i]) ) {
    							// Restore matcherIn since elem is not yet a final match
    							temp.push( (matcherIn[i] = elem) );
    						}
    					}
    					postFinder( null, (matcherOut = []), temp, xml );
    				}
    
    				// Move matched elements from seed to results to keep them synchronized
    				i = matcherOut.length;
    				while ( i-- ) {
    					if ( (elem = matcherOut[i]) &&
    						(temp = postFinder ? indexOf.call( seed, elem ) : preMap[i]) > -1 ) {
    
    						seed[temp] = !(results[temp] = elem);
    					}
    				}
    			}
    
    		// Add elements to results, through postFinder if defined
    		} else {
    			matcherOut = condense(
    				matcherOut === results ?
    					matcherOut.splice( preexisting, matcherOut.length ) :
    					matcherOut
    			);
    			if ( postFinder ) {
    				postFinder( null, results, matcherOut, xml );
    			} else {
    				push.apply( results, matcherOut );
    			}
    		}
    	});
    }
    
    function matcherFromTokens( tokens ) {
    	var checkContext, matcher, j,
    		len = tokens.length,
    		leadingRelative = Expr.relative[ tokens[0].type ],
    		implicitRelative = leadingRelative || Expr.relative[" "],
    		i = leadingRelative ? 1 : 0,
    
    		// The foundational matcher ensures that elements are reachable from top-level context(s)
    		matchContext = addCombinator( function( elem ) {
    			return elem === checkContext;
    		}, implicitRelative, true ),
    		matchAnyContext = addCombinator( function( elem ) {
    			return indexOf.call( checkContext, elem ) > -1;
    		}, implicitRelative, true ),
    		matchers = [ function( elem, context, xml ) {
    			return ( !leadingRelative && ( xml || context !== outermostContext ) ) || (
    				(checkContext = context).nodeType ?
    					matchContext( elem, context, xml ) :
    					matchAnyContext( elem, context, xml ) );
    		} ];
    
    	for ( ; i < len; i++ ) {
    		if ( (matcher = Expr.relative[ tokens[i].type ]) ) {
    			matchers = [ addCombinator(elementMatcher( matchers ), matcher) ];
    		} else {
    			matcher = Expr.filter[ tokens[i].type ].apply( null, tokens[i].matches );
    
    			// Return special upon seeing a positional matcher
    			if ( matcher[ expando ] ) {
    				// Find the next relative operator (if any) for proper handling
    				j = ++i;
    				for ( ; j < len; j++ ) {
    					if ( Expr.relative[ tokens[j].type ] ) {
    						break;
    					}
    				}
    				return setMatcher(
    					i > 1 && elementMatcher( matchers ),
    					i > 1 && toSelector(
    						// If the preceding token was a descendant combinator, insert an implicit any-element `*`
    						tokens.slice( 0, i - 1 ).concat({ value: tokens[ i - 2 ].type === " " ? "*" : "" })
    					).replace( rtrim, "$1" ),
    					matcher,
    					i < j && matcherFromTokens( tokens.slice( i, j ) ),
    					j < len && matcherFromTokens( (tokens = tokens.slice( j )) ),
    					j < len && toSelector( tokens )
    				);
    			}
    			matchers.push( matcher );
    		}
    	}
    
    	return elementMatcher( matchers );
    }
    
    function matcherFromGroupMatchers( elementMatchers, setMatchers ) {
    	var bySet = setMatchers.length > 0,
    		byElement = elementMatchers.length > 0,
    		superMatcher = function( seed, context, xml, results, outermost ) {
    			var elem, j, matcher,
    				matchedCount = 0,
    				i = "0",
    				unmatched = seed && [],
    				setMatched = [],
    				contextBackup = outermostContext,
    				// We must always have either seed elements or outermost context
    				elems = seed || byElement && Expr.find["TAG"]( "*", outermost ),
    				// Use integer dirruns iff this is the outermost matcher
    				dirrunsUnique = (dirruns += contextBackup == null ? 1 : Math.random() || 0.1),
    				len = elems.length;
    
    			if ( outermost ) {
    				outermostContext = context !== document && context;
    			}
    
    			// Add elements passing elementMatchers directly to results
    			// Keep `i` a string if there are no elements so `matchedCount` will be "00" below
    			// Support: IE<9, Safari
    			// Tolerate NodeList properties (IE: "length"; Safari: <number>) matching elements by id
    			for ( ; i !== len && (elem = elems[i]) != null; i++ ) {
    				if ( byElement && elem ) {
    					j = 0;
    					while ( (matcher = elementMatchers[j++]) ) {
    						if ( matcher( elem, context, xml ) ) {
    							results.push( elem );
    							break;
    						}
    					}
    					if ( outermost ) {
    						dirruns = dirrunsUnique;
    					}
    				}
    
    				// Track unmatched elements for set filters
    				if ( bySet ) {
    					// They will have gone through all possible matchers
    					if ( (elem = !matcher && elem) ) {
    						matchedCount--;
    					}
    
    					// Lengthen the array for every element, matched or not
    					if ( seed ) {
    						unmatched.push( elem );
    					}
    				}
    			}
    
    			// Apply set filters to unmatched elements
    			matchedCount += i;
    			if ( bySet && i !== matchedCount ) {
    				j = 0;
    				while ( (matcher = setMatchers[j++]) ) {
    					matcher( unmatched, setMatched, context, xml );
    				}
    
    				if ( seed ) {
    					// Reintegrate element matches to eliminate the need for sorting
    					if ( matchedCount > 0 ) {
    						while ( i-- ) {
    							if ( !(unmatched[i] || setMatched[i]) ) {
    								setMatched[i] = pop.call( results );
    							}
    						}
    					}
    
    					// Discard index placeholder values to get only actual matches
    					setMatched = condense( setMatched );
    				}
    
    				// Add matches to results
    				push.apply( results, setMatched );
    
    				// Seedless set matches succeeding multiple successful matchers stipulate sorting
    				if ( outermost && !seed && setMatched.length > 0 &&
    					( matchedCount + setMatchers.length ) > 1 ) {
    
    					Sizzle.uniqueSort( results );
    				}
    			}
    
    			// Override manipulation of globals by nested matchers
    			if ( outermost ) {
    				dirruns = dirrunsUnique;
    				outermostContext = contextBackup;
    			}
    
    			return unmatched;
    		};
    
    	return bySet ?
    		markFunction( superMatcher ) :
    		superMatcher;
    }
    
    compile = Sizzle.compile = function( selector, match /* Internal Use Only */ ) {
    	var i,
    		setMatchers = [],
    		elementMatchers = [],
    		cached = compilerCache[ selector + " " ];
    
    	if ( !cached ) {
    		// Generate a function of recursive functions that can be used to check each element
    		if ( !match ) {
    			match = tokenize( selector );
    		}
    		i = match.length;
    		while ( i-- ) {
    			cached = matcherFromTokens( match[i] );
    			if ( cached[ expando ] ) {
    				setMatchers.push( cached );
    			} else {
    				elementMatchers.push( cached );
    			}
    		}
    
    		// Cache the compiled function
    		cached = compilerCache( selector, matcherFromGroupMatchers( elementMatchers, setMatchers ) );
    
    		// Save selector and tokenization
    		cached.selector = selector;
    	}
    	return cached;
    };
    
    /**
     * A low-level selection function that works with Sizzle's compiled
     *  selector functions
     * @param {String|Function} selector A selector or a pre-compiled
     *  selector function built with Sizzle.compile
     * @param {Element} context
     * @param {Array} [results]
     * @param {Array} [seed] A set of elements to match against
     */
    select = Sizzle.select = function( selector, context, results, seed ) {
    	var i, tokens, token, type, find,
    		compiled = typeof selector === "function" && selector,
    		match = !seed && tokenize( (selector = compiled.selector || selector) );
    
    	results = results || [];
    
    	// Try to minimize operations if there is no seed and only one group
    	if ( match.length === 1 ) {
    
    		// Take a shortcut and set the context if the root selector is an ID
    		tokens = match[0] = match[0].slice( 0 );
    		if ( tokens.length > 2 && (token = tokens[0]).type === "ID" &&
    				support.getById && context.nodeType === 9 && documentIsHTML &&
    				Expr.relative[ tokens[1].type ] ) {
    
    			context = ( Expr.find["ID"]( token.matches[0].replace(runescape, funescape), context ) || [] )[0];
    			if ( !context ) {
    				return results;
    
    			// Precompiled matchers will still verify ancestry, so step up a level
    			} else if ( compiled ) {
    				context = context.parentNode;
    			}
    
    			selector = selector.slice( tokens.shift().value.length );
    		}
    
    		// Fetch a seed set for right-to-left matching
    		i = matchExpr["needsContext"].test( selector ) ? 0 : tokens.length;
    		while ( i-- ) {
    			token = tokens[i];
    
    			// Abort if we hit a combinator
    			if ( Expr.relative[ (type = token.type) ] ) {
    				break;
    			}
    			if ( (find = Expr.find[ type ]) ) {
    				// Search, expanding context for leading sibling combinators
    				if ( (seed = find(
    					token.matches[0].replace( runescape, funescape ),
    					rsibling.test( tokens[0].type ) && testContext( context.parentNode ) || context
    				)) ) {
    
    					// If seed is empty or no tokens remain, we can return early
    					tokens.splice( i, 1 );
    					selector = seed.length && toSelector( tokens );
    					if ( !selector ) {
    						push.apply( results, seed );
    						return results;
    					}
    
    					break;
    				}
    			}
    		}
    	}
    
    	// Compile and execute a filtering function if one is not provided
    	// Provide `match` to avoid retokenization if we modified the selector above
    	( compiled || compile( selector, match ) )(
    		seed,
    		context,
    		!documentIsHTML,
    		results,
    		rsibling.test( selector ) && testContext( context.parentNode ) || context
    	);
    	return results;
    };
    
    // One-time assignments
    
    // Sort stability
    support.sortStable = expando.split("").sort( sortOrder ).join("") === expando;
    
    // Support: Chrome 14-35+
    // Always assume duplicates if they aren't passed to the comparison function
    support.detectDuplicates = !!hasDuplicate;
    
    // Initialize against the default document
    setDocument();
    
    // Support: Webkit<537.32 - Safari 6.0.3/Chrome 25 (fixed in Chrome 27)
    // Detached nodes confoundingly follow *each other*
    support.sortDetached = assert(function( div1 ) {
    	// Should return 1, but returns 4 (following)
    	return div1.compareDocumentPosition( document.createElement("div") ) & 1;
    });
    
    // Support: IE<8
    // Prevent attribute/property "interpolation"
    // http://msdn.microsoft.com/en-us/library/ms536429%28VS.85%29.aspx
    if ( !assert(function( div ) {
    	div.innerHTML = "<a href='#'></a>";
    	return div.firstChild.getAttribute("href") === "#" ;
    }) ) {
    	addHandle( "type|href|height|width", function( elem, name, isXML ) {
    		if ( !isXML ) {
    			return elem.getAttribute( name, name.toLowerCase() === "type" ? 1 : 2 );
    		}
    	});
    }
    
    // Support: IE<9
    // Use defaultValue in place of getAttribute("value")
    if ( !support.attributes || !assert(function( div ) {
    	div.innerHTML = "<input/>";
    	div.firstChild.setAttribute( "value", "" );
    	return div.firstChild.getAttribute( "value" ) === "";
    }) ) {
    	addHandle( "value", function( elem, name, isXML ) {
    		if ( !isXML && elem.nodeName.toLowerCase() === "input" ) {
    			return elem.defaultValue;
    		}
    	});
    }
    
    // Support: IE<9
    // Use getAttributeNode to fetch booleans when getAttribute lies
    if ( !assert(function( div ) {
    	return div.getAttribute("disabled") == null;
    }) ) {
    	addHandle( booleans, function( elem, name, isXML ) {
    		var val;
    		if ( !isXML ) {
    			return elem[ name ] === true ? name.toLowerCase() :
    					(val = elem.getAttributeNode( name )) && val.specified ?
    					val.value :
    				null;
    		}
    	});
    }
    
    // EXPOSE
    if ( typeof define === "function" && define.amd ) {
    	define(function() { return Sizzle; });
    // Sizzle requires that there be a global window in Common-JS like environments
    } else if ( typeof module !== "undefined" && module.exports ) {
    	module.exports = Sizzle;
    } else {
    	window.Sizzle = Sizzle;
    }
    // EXPOSE
    
    })( window );
    (function(i18n)
    {
        var init = function(destination, source) { for (var property in source) { destination[property] = source[property]; } return destination; };
    
        i18n = init(i18n, {
            /* 常用 */
            generic: {
                filter: '查询',
                query: '查询',
                add: '新增',
                edit: '编辑',
                remove: '删除',
                refresh: '刷新'
            },
    
            /* 日期 */
            date: {
                dateformat: {
                    "fulldaykey": "yyyyMMdd",
                    "fulldayshow": "yyyy年M月d日",
                    "fulldayvalue": "yyyy-M-d",
                    "Md": "M/d (W)",
                    "Md3": "M月d日"
                }
            },
            /* 元素 */
            dom: {
                errors: {
                }
            },
            /* 网络 */
            net: {
                errors: {
                    401: '访问被拒绝，客户试图未经授权访问受密码保护的页面。',
                    404: '无法找到指定位置的资源。',
                    500: '服务器繁忙，请稍候再试。',
                    unkown: '系统错误，错误信息【{0}】。'
                },
    
                waiting: {
                    queryTipText: '正在查询数据，请稍后......',
                    commitTipText: '正在提交数据，请稍候......',
                    saveTipText: '正在保存数据，请稍候......',
                    deleteTipText: '正在删除数据，请稍候......'
                }
            }
        });
    
        window.i18n = i18n;
    
        return i18n;
    })(typeof i18n !== 'undefined' ? i18n : {});// -*- ecoding=utf-8 -*-
    
    /**
    * @namespace x
    * @description 默认根命名空间
    */
    var x = {
    
        // 默认设置
        defaults: {
            // 默认消息提示方式
            msg: function(text) { if (alert) { alert(text); } else { console.log(text); } }
        },
    
        // 缓存
        cache: {},
    
        /*#region 函数:msg(text)*/
        msg: function(text) { x.defaults.msg(text); },
        /*#endregion*/
    
        /*#region 函数:type(object)*/
        /**
        * 检查对象类型
        * @method type
        * @memberof x
        */
        type: function(object)
        {
            try
            {
                if (typeof (object) === 'undefined') { return 'undefined'; }
                if (object === null) { return 'null'; }
    
                return /\[object ([A-Za-z]+)\]/.exec(Object.prototype.toString.call(object))[1].toLowerCase();
            }
            catch (ex)
            {
                if (ex instanceof RangeError) { return '...'; }
    
                throw ex;
            }
        },
        /*#endregion*/
    
        /*#region 函数:isArray(object)*/
        /**
        * 判断对象是否是 Array 类型
        * @method isArray
        * @memberof x
        */
        isArray: function(object)
        {
            return x.type(object) === 'array';
        },
        /*#endregion*/
    
        /*#region 函数:isFunction(object)*/
        /**
        * 判断对象是否是 Function 类型
        * @method isFunction
        * @memberof x
        */
        isFunction: function(object)
        {
            return x.type(object) === 'function';
        },
        /*#endregion*/
    
        /*#region 函数:isString(object)*/
        /**
        * 判断对象是否是 String 类型
        * @method isString
        * @memberof x
        */
        isString: function(object)
        {
            return x.type(object) === 'string';
        },
        /*#endregion*/
    
        /*#region 函数:isNumber(object)*/
        /**
        * 判断对象是否是 Number 类型
        * @method inspect
        * @memberof Object
        */
        isNumber: function(object)
        {
            return x.type(object) === 'number';
        },
        /*#endregion*/
    
        /*#region 函数:isUndefined(value, replacementValue)*/
        /**
        * 判断是否是 undefined 类型, 如果设置了替换的值, 则当第一个参数为 undefined, 则使用替换的值
        * @method isUndefined
        * @memberof x
        * @param {object} value 值
        * @param {string} [replacementValue] 替换的值
        * @example
        * // return true
        * x.isUndefined(undefinedValue);    
        * @example
        * // return ''
        * x.isUndefined(undefinedValue, '');    
        */
        isUndefined: function(object, replacementValue)
        {
            if (arguments.length == 2)
            {
                // 如果设置了 replacementValue 值, 则当对象是 undefined 值时, 返回替换值信息
                return (x.type(object) === 'undefined') ? replacementValue : object;
            }
            else
            {
                return x.type(object) === 'undefined';
            }
        },
        /*#endregion*/
    
        // 脚本代码片段
        scriptFragment: '<script[^>]*>([\\S\\s]*?)<\/script>',
    
        // 脚本代码片段
        jsonFilter: /^\/\*-secure-([\s\S]*)\*\/\s*$/,
    
        // 一种简单的方法来检查HTML字符串或ID字符串
        quickExpr: /^[^<]*(<(.|\s)+>)[^>]*$|^#([\w-]+)$/,
    
        // Is it a simple selector
        isSimple: /^.[^:#\[\.,]*$/,
    
        /*#region 函数:noop()*/
        /**
        * 空操作
        */
        noop: function() { },
        /*#endregion*/
    
        /*#region 函数:register(value)*/
        /**
        * 注册对象信息
        * @method register
        * @memberof x3platform
        */
        register: function(value)
        {
            var parts = value.split(".");
    
            var root = window;
    
            for (var i = 0; i < parts.length; i++)
            {
                if (x.isUndefined(root[parts[i]]))
                {
                    root[parts[i]] = {};
                }
    
                root = root[parts[i]];
            }
    
            return root;
        },
        /*#endregion*/
    
        /*#region 函数:ext(destination, source)*/
        /**
        * 将原始对象的属性和方法扩展至目标对象
        * @method ext
        * @memberof x
        * @param destination 目标对象
        * @param source 原始对象
        */
        ext: function(destination, source)
        {
            /*
            for (var property in source)
            {
            destination[property] = source[property];
            }
    
            return destination;
            */
    
            var result = arguments[0] || {};
    
            if (arguments.length > 1)
            {
                for (var i = 1; i < arguments.length; i++)
                {
                    for (var property in arguments[i])
                    {
                        result[property] = arguments[i][property];
                    }
                }
            }
    
            return result;
        },
        /*#endregion*/
    
        /*#region 函数:clone(object)*/
        /**
        * 克隆对象
        * @method clone
        * @memberof x
        * @returns {object} 克隆的对象
        */
        clone: function(object)
        {
            return x.ext({}, object);
        },
        /*#endregion*/
    
        /*#region 函数:invoke(object, fn)*/
        /**
        * 执行对象方法
        * @method invoke
        * @memberof x
        */
        invoke: function(object, fn)
        {
            // 注:数组的 slice(start, end) 方法可从已有的数组中返回选定的元素。
            var args = Array.prototype.slice.call(arguments).slice(2);
    
            return fn.apply(object, args);
        },
        /*#endregion*/
    
        /*#region 函数:call(anything)*/
        /*
        * 调用方法或者代码文本
        * @method call
        * @memberof x
        */
        call: function(anything)
        {
            if (!x.isUndefined(anything))
            {
                try
                {
                    if (x.isFunction(anything))
                    {
                        var args = Array.prototype.slice.call(arguments).slice(1);
    
                        return anything.apply(this, args);
                    }
                    else if (x.type(anything) === 'string')
                    {
                        if (anything !== '') { return eval(anything); }
                    }
                }
                catch (ex)
                {
                    x.debug.error(ex);
                }
            }
        },
        /*#endregion*/
    
        /*#region 函数:query(selector)*/
        /**
        * 精确查询单个表单元素。
        * @method query
        * @memberof x
        * @param {string} selector 选择表达式
        */
        query: function(selector)
        {
            if (x.type(selector).indexOf('html') == 0)
            {
                // Html 元素类型 直接返回
                return selector;
            }
            else if (x.type(selector) == 'string')
            {
                var results = Sizzle.apply(window, Array.prototype.slice.call(arguments, 0));
    
                return (results.length == 0) ? null : results[0];
            }
        },
        /*#endregion*/
    
        /*#region 函数:queryAll(selector)*/
        /**
        * 精确查询单个表单元素。
        * @method query
        * @memberof x
        * @param {string} selector 选择表达式
        */
        queryAll: function(selector)
        {
            if (x.type(selector).indexOf('html') == 0)
            {
                // Html 元素类型 直接返回
                var results = [];
                results.push(selector);
    
                return results;
            }
            else if (x.type(selector) == 'string')
            {
                return Sizzle.apply(window, Array.prototype.slice.call(arguments, 0));
            }
        },
        /*#endregion*/
    
        /*#region 函数:serialize(data)*/
        /**
        * 返回数据串行化后的字符串  
        * @method serialize
        * @memberof x
        * @param {object} data 表单输入元素的数组或键/值对的散列表
        */
        serialize: function(data)
        {
            var buffer = [], length = data.length;
    
            if (x.isArray(data))
            {
                // 数组对象
                for (var i = 0; i < length; i++)
                {
                    buffer.push(data[i].name + '=' + encodeURIComponent(data[i].value));
                }
            }
            else
            {
                for (var name in data)
                {
                    buffer.push(name + '=' + encodeURIComponent(data[name]));
                }
            }
    
            return buffer.join('&');
        },
        /*#endregion*/
    
        /*#region 函数:each(data, callback)*/
        /**
        * 遍历元素对象, 如果需要退出返回 false
        * @method query
        * @memberof x
        * @param {Object} data 对象
        * @param {Function} callback 回调函数
        */
        each: function(data, callback)
        {
            var name, i = 0, length = data.length;
    
            if (x.isArray(data))
            {
                // 数组对象
                for (var value = data[0]; i < length && callback.call(value, i, value) != false; value = data[++i]) { }
            }
            else
            {
                // 键/值对的散列表
                for (name in data)
                {
                    if (callback.call(data[name], name, data[name]) === false) { break; }
                }
            }
    
            return data;
        },
        /*#endregion*/
    
        /*#region 函数:toXML(text)*/
        /**
        * 将字符串转换为XML对象
        * @method toXML
        * @memberof x
        * @param {string} text XML对象的文本格式
        */
        toXML: function(text)
        {
            if (x.type(text) === 'xmldocument') { return text; }
    
            // 类型为 undefined 时或者字符串内容为空时, 返回 undefined 值.
            if (x.isUndefined(text) || text === '') { return undefined; }
    
            var hideError = !!arguments[1];
    
            var doc;
    
            // Firefox, Mozilla, Opera, etc.
            try
            {
                if (window.DOMParser)
                {
                    var parser = new DOMParser();
                    doc = parser.parseFromString(text, "text/xml");
                }
                else if (window.ActiveXObject)
                {
                    doc = new ActiveXObject("Microsoft.XMLDOM");
                    doc.async = "false";
                    doc.loadXML(text);
                }
            }
            catch (ex)
            {
                doc = undefined;
    
                if (!hideError) x.debug.error('{"method":"x.toXML(text)", "arguments":{"text":"' + text + '"}');
            }
    
            if (!doc || doc.getElementsByTagName("parsererror").length)
            {
                doc = undefined;
                if (!hideError) x.debug.error('{"method":"x.toXML(text)", "arguments":{"text":"' + text + '"}');
            }
    
            return doc;
        },
        /*#endregion*/
    
        /*#region 函数:toJSON(text)*/
        /**
        * 将字符串转换为JSON对象
        * @method toJSON
        * @memberof x
        * @param {string} text JSON对象的文本格式
        */
        toJSON: function(text)
        {
            if (x.type(text) === 'object') { return text; }
    
            // 类型为 undefined 时或者字符串内容为空时, 返回 undefined 值.
            if (x.isUndefined(text) || text === '') { return undefined; }
    
            var hideError = arguments[1];
    
            try
            {
                // eval('(' + text + ')')
                return (JSON) ? JSON.parse(text) : (Function("return " + text))();
            }
            catch (ex)
            {
                try
                {
                    return (Function("return " + text))();
                }
                catch (ex1)
                {
                    if (!hideError) x.debug.error('{"method":"x.toJSON(text)", "arguments":{"text":"' + text + '"}');
                    return undefined;
                }
            }
        },
        /*#endregion*/
    
        /*#region 函数:toSafeJSON(text)*/
        /**
        * 将普通文本信息转换为安全的符合JSON格式规范的文本信息
        * @method toSafeJSON
        * @memberof x
        * @param {string} text 文本信息
        */
        toSafeJSON: function(text)
        {
            var outString = '';
    
            for (var i = 0; i < text.length; i++)
            {
                var ch = text.substr(i, 1);
    
                if (ch === '"' || ch === '\'' || ch === '\\')
                {
                    outString += '\\';
                    outString += ch;
                }
                else if (ch === '\b')
                {
                    outString += '\\b';
                }
                else if (ch === '\f')
                {
                    outString += '\\f';
                }
                else if (ch === '\n')
                {
                    outString += '\\n';
                }
                else if (ch === '\t')
                {
                    outString += '\\t';
                }
                else if (ch === '\r')
                {
                    outString += '\\r';
                }
                else
                {
                    outString += ch;
                }
            }
    
            return outString;
        },
        /*#endregion*/
    
        /*#region 函数:toSafeLike(text)*/
        /**
        * 将字符串中特殊字符([%_)转换为可识别的Like内容.
        * @method toSafeLike
        * @memberof x
        * @param {string} text 文本信息
        */
        toSafeLike: function(text)
        {
            return text.replace(/\[/g, '[[]').replace(/%/g, '[%]').replace(/_/g, '[_]');
        },
        /*#endregion*/
    
        /*#region 函数:cdata(text)*/
        /**
        * 将普通文本信息转为为Xml不解析的文本信息
        * @method cdata
        * @memberof x
        * @param {string} text 文本信息
        */
        cdata: function(text)
        {
            return '<![CDATA[' + text + ']]>';
        },
        /*#endregion*/
    
        /*#region 函数:camelCase(text)*/
        /**
        * 将短划线文字转换至驼峰格式
        * @method camelCase
        * @memberof x
        * @param {string} text 文本信息
        */
        camelCase: function(text)
        {
            // jQuery: Microsoft forgot to hump their vendor prefix (#9572)
    
            // 匹配短划线文字转换至驼峰格式
            // Matches dashed string for camelizing
            var rmsPrefix = /^-ms-/, rdashAlpha = /-([\da-z])/gi;
    
            // camelCase 替换字符串时的回调函数
            return text.replace(rmsPrefix, "ms-").replace(rdashAlpha, function(all, letter)
            {
                return letter.toUpperCase();
            });
        },
        /*#endregion*/
    
        /*#region 函数:paddingZero(number, length)*/
        /**
        * 数字补零
        * @method paddingZero
        * @memberof x
        * @param {number} number 数字
        * @param {number} length 需要补零的位数
        */
        paddingZero: function(number, length)
        {
            return (Array(length).join('0') + number).slice(-length);
        },
        /*#endregion*/
    
        /*#region 函数:formatNature(text)*/
        /**
        * 将字符串统一转换为本地标识标识
        * @method formatNature
        * @memberof x
        * @param {string} text 文本信息
        */
        formatNature: function(text)
        {
            switch (text.toLowerCase())
            {
                case 'en-us':
                    text = 'en-us';
                    break;
                case 'zh-cn':
                    text = 'zh-cn';
                    break;
                default:
                    text = 'zh-cn';
                    break;
            }
    
            return text;
        },
        /*#endregion*/
    
        /*#region 函数:getFriendlyName(name)*/
        /**
        * 将不规范的标识名称转换为友好的名称.
        * @method getFriendlyName
        * @memberof x
        * @param {string} name 名称
        * @example
        * // 将路径中的[$./\]符号替换为[-]符号
        * console.log(x.getFriendlyName(location.pathname));
        */
        getFriendlyName: function(name)
        {
            return x.camelCase(('x-' + name).replace(/[\#\$\.\/\\]/g, '-').replace(/[-]+/g, '-'));
        },
        /*#endregion*/
    
        /*#region 类:newHashTable()*/
        /**
        * 哈希表
        * @class HashTable 哈希表
        * @constructor newHashTable
        * @memberof x
        * @example
        * // returns HashTable
        * var hashtable = x.newHashTable();
        */
        newHashTable: function()
        {
            var hashTable = {
    
                // 内部数组对象
                innerArray: [],
    
                /*#region 函数:clear()*/
                /**
                * 清空哈希表
                * @method clear
                * @memberof x.newHashTable#
                */
                clear: function()
                {
                    this.innerArray = [];
                },
                /*#endregion*/
    
                /*#region 函数:exist(key)*/
                /**
                * 判断是否已存在相同键的对象
                * @method exist
                * @memberof x.newHashTable#
                * @returns {bool}
                */
                exist: function(key)
                {
                    for (var i = 0; i < this.innerArray.length; i++)
                    {
                        if (this.innerArray[i].name === key)
                        {
                            return true;
                        }
                    }
    
                    return false;
                },
                /*#endregion*/
    
                /*#region 函数:get(index)*/
                /**
                * @method get
                * @memberof x.newHashTable#
                */
                get: function(index)
                {
                    return this.innerArray[index];
                },
                /*#endregion*/
    
                /*#region 函数:add(key, value)*/
                /**
                * @method add
                * @memberof x.newHashTable#
                */
                add: function(key, value)
                {
                    if (arguments.length === 1)
                    {
                        var keyArr = key.split(';');
    
                        for (var i = 0; i < keyArr.length; i++)
                        {
                            var valueArr = keyArr[i].split('#');
    
                            this.innerArray.push(x.types.newListItem(valueArr[0], valueArr[1]));
    
                        }
                    }
                    else
                    {
                        if (this.exist(key))
                        {
                            throw 'hashtable aleardy exist same key[' + key + ']';
                        }
                        else
                        {
                            this.innerArray.push(x.types.newListItem(key, value));
                        }
                    }
                },
                /*#endregion*/
    
                /*#region 函数:find(key)*/
                /**
                * @method find
                * @memberof x.newHashTable#
                */
                find: function(key)
                {
                    for (var i = 0; i < this.innerArray.length; i++)
                    {
                        if (this.innerArray[i].name === key)
                        {
                            return this.innerArray[i].value;
                        }
                    }
    
                    return null;
                },
                /*#endregion*/
    
                /*#region 函数:size()*/
                /**
                * 获取哈希表的当前大小
                * @method size
                * @memberof x.newHashTable#
                */
                size: function()
                {
                    return this.innerArray.length;
                }
                /*#endregion*/
            };
    
            return hashTable;
        },
        /*#endregion*/
    
        /*#region 类:newQueue()*/
        /**
        * 队列
        * @description Queue 对象
        * @class Queue 队列
        * @constructor newQueue
        * @memberof x
        */
        newQueue: function()
        {
            var queue = {
    
                // 内部数组对象
                innerArray: [],
    
                /**
                * 插入队列顶部元素
                * @method push
                * @memberof x.newQueue#
                */
                push: function(targetObject)
                {
                    this.innerArray.push(targetObject);
                },
                /*#endregion*/
    
                /**
                * 弹出队列顶部元素
                * @method pop
                * @memberof x.newQueue#
                */
                pop: function()
                {
                    if (this.innerArray.length === 0)
                    {
                        return null;
                    }
                    else
                    {
                        var targetObject = this.innerArray[0];
    
                        // 将队列元素往前移动一个单位
                        for (var i = 0; i < this.innerArray.length - 1; i++)
                        {
                            this.innerArray[i] = this.innerArray[i + 1];
                        }
    
                        this.innerArray.length = this.innerArray.length - 1;
    
                        return targetObject;
                    }
                },
                /*#endregion*/
    
                /**
                * 取出队列底部元素(并不删除队列底部元素)
                */
                peek: function()
                {
                    if (this.innerArray.length === 0)
                    {
                        return null;
                    }
    
                    return this.innerArray[0];
                },
                /*#endregion*/
    
                /*#region 函数:clear()*/
                /**
                * 清空堆栈
                <<<<<<< HEAD
                * @method clear
                =======
                * @method isEmpty
                >>>>>>> 86d619ad16f6d4840df8ba2f3eaae9c8014fd094
                * @memberof x.newQueue#
                */
                clear: function()
                {
                    this.innerArray.length = 0; //将元素的个数清零即可
                },
                /*#endregion*/
    
                /*#region 函数:size()*/
                /**
                * 获得线性队列当前大小
                * @method size
                * @memberof x.newQueue#
                */
                size: function()
                {
                    return this.innerArray.length;
                },
                /*#endregion*/
    
                /*#region 函数:isEmpty()*/
                /*
                * 判断一个线性队列是否为空
                * @method isEmpty
                * @memberof x.newQueue#
                */
                isEmpty: function()
                {
                    return (this.innerArray.length === 0) ? true : false;
                }
                /*#endregion*/
            };
    
            return queue;
        },
        /*#endregion*/
    
        /*#region 类:newStack()*/
        /**
        * 栈
        * @description 创建 Stack 对象
        * @class Stack
        * @constructor newStack
        * @memberof x
        */
        newStack: function()
        {
            var stack = {
    
                // 内部数组对象
                innerArray: [],
    
                /*
                * 插入栈顶元素
                */
                push: function(targetObject)
                {
                    this.innerArray[this.innerArray.length] = targetObject;
                },
                /*#endregion*/
    
                /*
                * 弹出栈顶元素(并删除栈顶元素)
                */
                pop: function()
                {
                    if (this.innerArray.length === 0)
                    {
                        return null;
                    }
                    else
                    {
                        var targetObject = this.innerArray[this.innerArray.length - 1];
    
                        this.innerArray.length--;
    
                        return targetObject;
                    }
                },
                /*#endregion*/
    
                /*
                * 取出栈顶元素(并不删除栈顶元素)
                */
                peek: function()
                {
                    if (this.innerArray.length === 0)
                    {
                        return null;
                    }
    
                    return this.innerArray[this.innerArray.length - 1];
                },
                /*#endregion*/
    
                /*
                * 清空堆栈
                */
                clear: function()
                {
                    this.innerArray.length = 0; //将元素的个数清零即可
                },
                /*#endregion*/
    
                /*#region 函数:size()*/
                /**
                * 获得线性堆栈的当前大小
                * @method size
                * @memberof x.newStack#
                */
                size: function()
                {
                    return this.innerArray.length;
                },
                /*#endregion*/
    
                /*
                * 判断一个线性堆栈是否为空
                */
                isEmpty: function()
                {
                    return (this.innerArray.length === 0) ? true : false;
                }
                /*#endregion*/
            };
    
            return stack;
        },
        /*#endregion*/
    
        /*#region 类:newStringBuilder()*/
        /**
        * 高效字符串构建器<br />
        * 注: 现在的主流浏览器都针对字符串连接作了优化，所以性能要好于StringBuilder类，不推荐使用，仅作字符串算法研究。
        * @class StringBuilder
        * @constructor newStringBuilder
        * @memberof x
        */
        newStringBuilder: function()
        {
            var stringBuilder = {
    
                // 内部数组对象
                innerArray: [],
    
                /*#region 函数:append(text)*/
                /**
                * 附加文本信息
                * @method append
                * @memberof x.newStringBuilder#
                * @param {string} text 文本信息
                */
                append: function(text)
                {
                    this.innerArray[this.innerArray.length] = text;
                },
                /*#endregion*/
    
                /*#region 函数:toString()*/
                /**
                * 转换为字符串
                * @method toString
                * @memberof x.newStringBuilder#
                * @returns {string}
                */
                toString: function()
                {
                    return this.innerArray.join('');
                }
                /*#endregion*/
            };
    
            return stringBuilder;
        },
        /*#endregion*/
    
        /*#region 类:newTimer(interval, callback)*/
        /**
        * 计时器
        * @class Timer 计时器
        * @constructor newTimer
        * @memberof x
        * @param {number} interval 时间间隔(单位:秒)
        * @param {function} callback 回调函数
        * @example
        * // 初始化一个计时器
        * var timer = x.newTimer(5, function(timer) {
        *   console.log(new Date());
        *   // 启动计时器
        *   timer.stop();
        * });
        * // 停止计时器
        * timer.start();
        */
        newTimer: function(interval, callback)
        {
            var timer = {
    
                // 定时器的名称
                name: 'timer$' + Math.ceil(Math.random() * 900000000 + 100000000),
                // 定时器的间隔
                interval: interval * 1000,
                // 回调函数
                callback: callback,
    
                /*#region 函数:run()*/
                /**
                * 执行回调函数
                * @private
                * @method run
                * @memberof x.newTimer#
                */
                run: function()
                {
                    this.callback(this);
                },
                /*#endregion*/
    
                /*#region 函数:start()*/
                /**
                * 启动计时器
                * @method start
                * @memberof x.newTimer#
                */
                start: function()
                {
                    eval(this.name + ' = this;');
    
                    this.timerId = setInterval(this.name + '.run()', this.interval);
                },
                /*#endregion*/
    
                /*#region 函数:stop()*/
                /**
                * 停止计时器
                * @method stop
                * @memberof x.newTimer#
                */
                stop: function()
                {
                    clearInterval(this.timerId);
                }
                /*#endregion*/
            };
    
            return timer;
        },
        /*#endregion*/
    
        /**
        * 事件
        * @namespace event
        * @memberof x
        */
        event: {
            /*#region 函数:getEvent(event)*/
            /**
            * 获取事件对象
            * @method getEvent
            * @memberof x.event
            * @param {event} event 事件对象
            */
            getEvent: function(event)
            {
                return window.event ? window.event : event;
            },
            /*#endregion*/
    
            /*#region 函数:getTarget(event)*/
            /**
            * 获取事件的目标对象
            * @method getTarget
            * @memberof x.event
            * @param {event} event 事件对象
            */
            getTarget: function(event)
            {
                return window.event ? window.event.srcElement : (event ? event.target : null);
            },
            /*#endregion*/
    
            /*#region 函数:getPosition(event)*/
            /**
            * 获取事件的光标坐标
            * @method getPosition
            * @memberof x.event
            * @param {event} event 事件对象
            */
            getPosition: function(event)
            {
                var docElement = document.documentElement;
    
                var body = document.body || { scrollLeft: 0, scrollTop: 0 };
    
                return {
                    x: event.pageX || (event.clientX + (docElement.scrollLeft || body.scrollLeft) - (docElement.clientLeft || 0)),
                    y: event.pageY || (event.clientY + (docElement.scrollTop || body.scrollTop) - (docElement.clientTop || 0))
                };
            },
            /*#endregion*/
    
            /*#region 函数:preventDefault(event)*/
            /**
            * 停止事件传播
            * @method preventDefault
            * @memberof x.event
            * @param {event} event 事件对象
            */
            preventDefault: function(event)
            {
                // 如果提供了事件对象，则这是一个非IE浏览器 
                if (event && event.preventDefault)
                {
                    //阻止默认浏览器动作(W3C) 
                    event.preventDefault();
                }
                else
                {
                    //IE中阻止函数器默认动作的方式   
                    window.event.returnValue = false;
                }
            },
            /*#endregion*/
    
            /*#region 函数:stopPropagation(event)*/
            /**
            * 停止事件传播
            * @method stopPropagation
            * @memberof x.event
            * @param {event} event 事件对象
            */
            stopPropagation: function(event)
            {
                // 判定是否支持触摸
                //            suportsTouch = ("createTouch" in document);
    
                //            var touch = suportsTouch ? event.touches[0] : event;
    
                //            if (suportsTouch)
                //            {
                //                touch.stopPropagation();
                //                touch.preventDefault();
                //            }
                //            else
                //            {
    
                //如果提供了事件对象，则这是一个非IE浏览器  
                if (event && event.stopPropagation)
                {
                    //因此它支持W3C的stopPropagation()方法  
                    event.stopPropagation();
                }
                else
                {
                    //否则，我们需要使用IE的方式来取消事件冒泡   
                    window.event.cancelBubble = true;
                }
                return false;
            },
            /*#endregion*/
    
            /*#region 函数:add(target, type, listener, useCapture)*/
            /**
            * 添加事件监听器
            * @method add
            * @memberof x.event
            * @param {string} target 监听对象
            * @param {string} type 监听事件
            * @param {string} listener 处理函数
            * @param {string} [useCapture] 监听顺序方式
            */
            add: function(target, type, listener, useCapture)
            {
                if (target == null) return;
    
                if (target.addEventListener)
                {
                    target.addEventListener(type, listener, useCapture);
                }
                else if (target.attachEvent)
                {
                    target.attachEvent('on' + type, listener);
                }
                else
                {
                    target['on' + type] = listener;
                }
            },
            /*#endregion*/
    
            /*#region 函数:remove(target, type, listener, useCapture)*/
            /**
            * 移除事件监听器
            * @method remove
            * @memberof x.event
            * @param {string} target 监听对象
            * @param {string} type 监听事件
            * @param {string} listener 处理函数
            * @param {string} [useCapture] 监听顺序方式
            */
            remove: function(target, type, listener, useCapture)
            {
                if (target == null) return;
    
                if (target.removeEventListener)
                {
                    target.removeEventListener(type, listener, useCapture);
                }
                else if (target.detachEvent)
                {
                    target.detachEvent('on' + type, listener);
                }
                else
                {
                    target['on' + type] = null;
                }
            }
            /*#endregion*/
        },
    
        /**
        * Guid 格式文本
        * @namespace guid
        * @memberof x
        */
        guid: {
            /*#region 函数:create(format, isUpperCase)*/
            /**
            * 创建 Guid 格式文本
            * @method create
            * @memberof x.guid
            * @param {string} [format] 分隔符格式(如果填空白字符串则不显示)
            * @param {bool} [isUpperCase] 是否是大写格式(true|false)
            * @example
            * // 输出格式 aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa
            * console.log(x.guid.create());
            * @example
            * // 输出格式 aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            * console.log(x.guid.create(''));
            * @example
            * // 输出格式 AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA
            * console.log(x.guid.create('-', true));
            */
            create: function(format, isUpperCase)
            {
                var text = '';
    
                // 格式限制
                format = x.isUndefined(format, '-').toLowerCase();
    
                for (var i = 0; i < 8; i++)
                {
                    text += (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    
                    if (i > 0 && i < 5)
                    {
                        if (format === '-')
                        {
                            text += '-';
                        }
                    }
                }
    
                text = isUpperCase ? text.toUpperCase() : text.toLowerCase();
    
                return text;
            }
            /*#endregion*/
        },
    
        /**
        * 随机文本
        * @namespace randomText
        * @memberof x
        */
        randomText: {
            /*#region 函数:create(length)*/
            /**
            * 创建随机文本信息
            * @method create
            * @memberof x.randomText
            * @param {number} length 返回的文本长度
            * @example
            * // 输出格式 00000000
            * console.log(x.randomText.create(8));
            */
            create: function(length, buffer)
            {
                var result = '';
    
                var buffer = x.type(buffer) == 'string' ? buffer : "0123456789abcdefghijklmnopqrstuvwyzx";
    
                for (var i = 0; i < length; i++)
                {
                    result += buffer.charAt(Math.ceil(Math.random() * 100000000) % buffer.length);
                }
    
                return result;
            }
            /*#endregion*/
        },
    
        /**
        * 字符串
        * @namespace string
        * @memberof x
        */
        string: {
    
            /*#region 函数:stringify(value)*/
            /**
            * 将其他类型的值转换成字符串
            * @method stringify
            * @memberof x.string
            * @param {anything} value 值
            */
            stringify: function(value)
            {
                var type = x.type(value);
    
                if (type !== 'string')
                {
                    if (type === 'number')
                    {
                        value += '';
                    }
                    else if (type === 'function')
                    {
                        value = x.string.stringify(value.call(value));
                    }
                    else
                    {
                        value = '';
                    }
                }
    
                return value;
            },
            /*#endregion*/
    
            /*#region 函数:trim(text, trimText)*/
            /**
            * 去除字符串两端空白或其他文本信息
            * @method trim
            * @memberof x.string
            * @param {string} text 文本信息.
            * @param {number} [trimText] 需要去除的文本信息(默认为空白).
            */
            trim: function(text, trimText)
            {
                if (x.isUndefined(trimText))
                {
                    return text.replace(x.expressions.rules['trim'], '');
                }
                else
                {
                    return x.string.rtrim(x.string.ltrim(text, trimText), trimText);
                }
            },
            /*#endregion*/
    
            /*#region 函数:ltrim(text, trimText)*/
            /**
            * 去除字符串左侧空白.
            * @method ltrim
            * @memberof x.string
            * @param {string} text 文本信息.
            * @param {number} [trimText] 需要去除的文本信息(默认为空白).
            */
            ltrim: function(text, trimText)
            {
                if (x.isUndefined(trimText))
                {
                    return text.replace(/(^[\s\uFEFF\xA0]+)/g, '');
                }
                else
                {
                    return text.replace(RegExp('(^' + trimText + ')', 'gi'), '');
                }
            },
            /*#endregion*/
    
            /*#region 函数:rtrim(text, trimText)*/
            /**
            * 去除字符串右侧空白.
            * @method rtrim
            * @memberof x.string
            * @param {string} text 文本信息.
            * @param {number} [trimText] 需要去除的文本信息(默认为空白).
            */
            rtrim: function(text, trimText)
            {
                if (x.isUndefined(trimText))
                {
                    return text.replace(/([\s\uFEFF\xA0]+$)/g, '');
                }
                else
                {
                    return text.replace(RegExp('(' + trimText + '$)', 'gi'), '');
                    // return (text.substr(text.length - trimText.length, trimText.length) === trimText) ? text.substr(0, text.length - trimText.length) : text;
                }
            },
            /*#endregion*/
    
            /*#region 函数:format(text, args)*/
            /**
            * 去除字符串右侧空白.
            * @method rtrim
            * @memberof x.string
            * @param {string} text 文本信息.
            * @param {number} [trimText] 需要去除的文本信息(默认为空白).
            */
            format: function()
            {
                if (arguments.length == 0) { return null; }
    
                var text = arguments[0];
    
                for (var i = 1; i < arguments.length; i++)
                {
                    var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
                    text = text.replace(re, arguments[i]);
                }
    
                return text;
            },
            /*#endregion*/
    
            /*#region 函数:left(text, length, hasEllipsis)*/
            /**
            * 字符串长度超长时, 截取左侧字符
            * @method left
            * @memberof x.string
            * @param {string} text 需要处理的字符串
            * @param {number} length 长度范围
            * @param {bool} [hasEllipsis] 是否显示...
            * @example
            * // 返回 'java...'
            * x.string.left('javascript', 4);
            * @example
            * // 返回 'java'
            * x.string.left('javascript', 4, false);
            */
            left: function(text, length, hasEllipsis)
            {
                if (text.length === 0) { return text; }
    
                if (text.length > length)
                {
                    return text.substr(0, length) + (x.isUndefined(hasEllipsis, true) ? '...' : '');
                }
                else
                {
                    return text;
                }
            }
            /*#endregion*/
        },
    
        /**
        * 颜色编码
        * @namespace color
        * @memberof x
        */
        color: {
    
            // 正则规则
            // reg: /^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/,
    
            /**
            * RGB 颜色转为十六进制格式
            */
            hex: function(colorRgbCode)
            {
                if (/^(rgb|RGB)/.test(colorRgbCode))
                {
                    var colorBuffer = colorRgbCode.replace(/(?:\(|\)|rgb|RGB)*/g, "").split(",");
                    var strHex = "#";
                    for (var i = 0; i < colorBuffer.length; i++)
                    {
                        var hex = Number(colorBuffer[i]).toString(16);
    
                        if (hex === "0")
                        {
                            hex += hex;
                        }
    
                        strHex += hex;
                    }
    
                    if (strHex.length !== 7)
                    {
                        strHex = colorRgbCode;
                    }
    
                    return strHex;
                }
                else if (/^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/.test(colorRgbCode))
                {
                    var colorBuffer = colorRgbCode.replace(/#/, "").split("");
    
                    if (colorBuffer.length === 6)
                    {
                        return colorRgbCode;
                    }
                    else if (colorBuffer.length === 3)
                    {
                        var numHex = "#";
    
                        for (var i = 0; i < colorBuffer.length; i += 1)
                        {
                            numHex += (colorBuffer[i] + colorBuffer[i]);
                        }
    
                        return numHex;
                    }
                }
                else
                {
                    return colorRgbCode;
                }
            },
    
            /**
            * 十六进制颜色转为 RGB 格式
            */
            rgb: function(colorHexCode)
            {
                var color = colorHexCode.toLowerCase();
    
                if (color && /^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/.test(color))
                {
                    // 处理简写的颜色
                    if (color.length === 4)
                    {
                        var originalColor = "#";
    
                        for (var i = 1; i < 4; i += 1)
                        {
                            originalColor += color.slice(i, i + 1).concat(color.slice(i, i + 1));
                        }
    
                        color = originalColor;
                    }
    
                    // 处理六位的颜色值
                    var colorBuffer = [];
    
                    for (var i = 1; i < 7; i += 2)
                    {
                        colorBuffer.push(parseInt("0x" + color.slice(i, i + 2)));
                    }
                    return 'rgb(' + colorBuffer.join(', ') + ')';
                }
                else
                {
                    return color;
                }
            }
        }
    };
    
    // 获取脚本文件位置
    var scriptFilePath = '';
    
    x.file = function()
    {
        return scriptFilePath;
    };
    
    x.dir = function()
    {
        if (scriptFilePath.length > 0)
        {
            return scriptFilePath.substring(0, scriptFilePath.lastIndexOf("/") + 1);
        }
        else
        {
            return '';
        }
    };
    
    if (document)
    {
        try
        {
            // 判断文件路径的 javascript 代码一般都直接放在 javascript 文件中而不是函数中，
            // 所以当加载该 javascript 文件时会立即执行其中的语句，而执行此语句时所获取到的文件数目正好是scripts.length-1，
            // 因为页面后面的 javascript 文件还没有加载，所以该处的js文件获取的数目并不是页面所有的js文件的数目。
            var scripts = document.scripts;
    
            scriptFilePath = scripts[scripts.length - 1].src.replace(location.origin, '');
        }
        catch (ex)
        {
            scriptFilePath = '';
        }
    }
    
    /**
    * 加载脚本
    * @method require
    * @memberof x
    * @param {object} options 选项 
    */
    var require = x.require = function(options)
    {
        if (x.isArray(options.files))
        {
            var file, files = options.files;
    
            if (files.length > 0)
            {
                file = files.shift();
    
                if (files.length == 0)
                {
                    require.newRequire({
                        fileType: file.fileType,
                        id: file.id,
                        path: file.path,
                        data: options.data,
                        callback: options.callback
                    });
                }
                else if (files.length > 0)
                {
                    require.newRequire({
                        fileType: file.fileType,
                        id: file.id,
                        path: file.path,
                        data: options.data,
                        next: { files: files, callback: options.callback },
                        callback: function(context)
                        {
                            require({
                                files: context.next.files,
                                data: context.data,
                                callback: context.next.callback
                            });
                        }
                    });
                }
            }
        }
        else
        {
            require.newRequire({
                fileType: options.fileType,
                id: options.id,
                path: options.path,
                data: options.data,
                callback: options.callback
            });
        }
    };
    
    require.newRequire = function(options)
    {
        var context = x.ext({
            fileType: 'script',
            id: '',
            path: ''
        }, options || {});
    
        if (context.fileType == 'template')
        {
            if (context.next)
            {
                x.net.require({
                    fileType: context.fileType,
                    id: context.id,
                    path: context.path,
                    async: false,
                    callback: function()
                    {
                        require({
                            files: context.next.files,
                            data: context.data,
                            callback: context.next.callback
                        });
                    }
                });
            }
            else
            {
                x.net.require({
                    fileType: context.fileType,
                    id: context.id,
                    path: context.path,
                    async: false,
                    callback: context.callback
                });
            }
            return;
        }
    
        var load = function(node, fn)
        {
            //Check if node.attachEvent is artificially added by custom script or
            //natively supported by browser
            //read https://github.com/jrburke/requirejs/issues/187
            //if we can NOT find [native code] then it must NOT natively supported.
            //in IE8, node.attachEvent does not have toString()
            //Note the test for "[native code" with no closing brace, see:
            //https://github.com/jrburke/requirejs/issues/273
    
            if (node.attachEvent
                    && !(node.attachEvent.toString && node.attachEvent.toString().indexOf('[native code') < 0)
                    && !x.browser.opera)
            {
                x.event.add(node, 'readystatechange', fn);
            }
            else
            {
                x.event.add(node, 'load', fn);
            }
        };
    
        var onScriptLoad = function(event)
        {
            x.debug.log('require file {"id":"{0}", path:"{1}"} finished.'.format(context.id, context.path));
    
            var node = x.event.getTarget(event);
    
            if (event.type === 'load' || /^(complete|loaded)$/.test(node.readyState))
            {
                node.ready = true;
    
                if (x.isFunction(context.callback))
                {
                    context.callback(context);
                }
            }
        };
    
        var head = document.getElementsByTagName('head')[0];
    
        var node = document.getElementById(context.id);
    
        if (node == null)
        {
            // 未找到相关依赖资源文件
            if (context.fileType == 'css')
            {
                var node = document.createElement("link");
    
                node.id = context.id;
                node.type = "text/css";
                node.rel = "stylesheet";
                node.href = context.path;
            }
            else
            {
                var node = document.createElement("script");
    
                node.id = context.id;
                node.type = "text/javascript";
                node.async = true;
                node.src = context.path;
            }
    
            load(node, onScriptLoad);
    
            head.appendChild(node);
    
            x.debug.log('require file {"id":"{0}", path:"{1}"} loading.'.format(context.id, context.path));
        }
        else
        {
            // 存在相关依赖文件
            if (x.isFunction(context.callback))
            {
                context.callback(context);
            }
            else
            {
                load(node, onScriptLoad);
            }
    
            x.debug.log('require file {"id":"{0}", path:"{1}"} exist.'.format(options.id, options.path));
        }
    
        return context;
    };
    
    /**
    * JSONP 函数
    * @method require
    * @memberof x
    * @param {object} options 选项
    */
    x.jsonp = function(options)
    {
        var options = x.ext({
            fileType: 'javascipt',
            id: 'JSONP' + Number(new Date()),
            jsonp: 'callback',
            jsonpCallback: 'jsonpCallbackCallback'
        }, options);
    
        options.path = options.url + ((options.url.indexOf('?') == -1) ? '?' : '&') + options.jsonp + '=' + options.jsonpCallback;
    
        x.require(options);
    };
    
    /**
    * 浏览器
    * @namespace browser
    * @memberof x
    */
    x.browser = {
    
        /** 
        * 判断是否是 Internet Explorer 浏览器
        * @member {bool} ie 
        * @memberof x.browser
        * @example
        * // returns true or false
        * x.browser.ie;
        */
        ie: !!(window.attachEvent && navigator.userAgent.indexOf('Opera') === -1),
        /** 
        * 判断是否是 Internet Explorer 6 浏览器
        * @member {bool} ie6 
        * @memberof x.browser
        * @example
        * // returns true or false
        * x.browser.ie6;
        */
        ie6: ! -[1, ] && !window.XMLHttpRequest,
        /** 
        * 判断是否是 Webkit 浏览器
        * @member {bool} webkit 
        * @memberof x.browser
        * @example
        * // returns true or false
        * x.browser.webkit;
        */
        webkit: navigator.userAgent.indexOf('AppleWebKit/') > -1,
        /** 
        * 判断是否是 Gecko 浏览器
        * @member {bool} gecko 
        * @memberof x.browser
        * @example
        * // returns true or false
        * x.browser.gecko;
        */
        gecko: navigator.userAgent.indexOf('Gecko') > -1 && navigator.userAgent.indexOf('KHTML') === -1,
        /** 
        * 判断是否是 Opera 浏览器
        * @member {bool} opera 
        * @memberof x.browser
        * @example
        * // returns true or false
        * x.browser.opera;
        */
        opera: typeof opera !== 'undefined' && opera.toString() === '[object Opera]',
        /** 
        * 判断是否是 Mobile Safari 浏览器
        * @member {bool} mobilesafari 
        * @memberof x.browser
        * @example
        * // returns true or false
        * x.browser.mobilesafari;
        */
        mobilesafari: !!navigator.userAgent.match(/Apple.*Mobile.*Safari/),
    
        /*#region 函数:current()*/
        /** 
        * 获取当前浏览器的名称和版本
        * @method current 
        * @memberof x.browser
        * @example
        * x.browser.current();
        */
        current: function()
        {
            return { name: x.browser.getName(), version: x.browser.getVersion() };
        },
        /*#endregion*/
    
        /*#region 函数:getName()*/
        /** 
        * 获取当前浏览器的名称
        * @method getName 
        * @memberof x.browser
        * @example
        * x.browser.getName();
        */
        getName: function()
        {
            if (navigator.userAgent.indexOf("MSIE") > 0)
                return "Internet Explorer";
            if (navigator.userAgent.indexOf("Chrome") >= 0)
                return "Chrome";
            if (navigator.userAgent.indexOf("Firefox") >= 0)
                return "Firefox";
            if (navigator.userAgent.indexOf("Opera") >= 0)
                return "Opera";
            if (navigator.userAgent.indexOf("Safari") > 0)
                return "Safari";
            if (navigator.userAgent.indexOf("Camino") > 0)
                return "Camino";
            if (navigator.userAgent.indexOf("Gecko") > 0)
                return "Gecko";
    
            return "unknown";
        },
        /*#endregion*/
    
        /*#region 函数:getVersion()*/
        /** 
        * 获取当前浏览器的版本
        * @method getVersion 
        * @memberof x.browser
        * @example
        * x.browser.getVersion();
        */
        getVersion: function()
        {
            var browserName = x.browser.getName();
    
            var version = navigator.userAgent;
    
            var startValue;
            var lengthValue;
    
            switch (browserName)
            {
                case "Internet Explorer":
                    startValue = version.indexOf("MSIE") + 5;
                    lengthValue = 3;
                    version = version.substr(startValue, lengthValue);
                    break;
                case "Firefox":
                    startValue = version.indexOf("Firefox") + 8;
                    lengthValue = 3;
                    version = version.substr(startValue, lengthValue);
                    break;
                case "Opera":
                    startValue = version.indexOf("Opera") + 6;
                    lengthValue = 3;
                    version = version.substr(startValue, lengthValue);
                    break;
                case "Safari":
                    break;
                case "Camino":
                    break;
                case "Gecko":
                    break;
                default:
                    break;
            }
    
            return version;
        },
        /*#endregion*/
    
        /**
        * 浏览器特性
        * @namespace features
        * @memberof x.browser
        */
        features: {
            /**
            * Selector 特性, 支持 querySelector, querySelectorAll
            * @member selector
            * @memberof x.browser.features
            */
            selector: !!document.querySelector,
            /**
            * SuportTouch 特性, 支持触摸事件
            * @member suportTouch
            * @memberof x.browser.features
            */
            suportTouch: ("createTouch" in document),
            /**
            * XPath 特性
            * @member xpath
            * @memberof x.browser.features
            */
            xpath: !!document.evaluate,
    
            // 元素特性
            elementExtensions: !!window.HTMLElement,
            specificElementExtensions:
    		        document.createElement('div')['__proto__']
                    && document.createElement('div')['__proto__'] !== document.createElement('form')['__proto__']
        }
    };
    
    /**
    * @namespace ui
    * @memberof x
    * @description UI 名称空间
    */
    x.ui = {
    
        /**
        * 样式名称默认前缀
        * @member {string} classNamePrefix 样式名称默认前缀
        * @memberof x.ui
        */
        classNamePrefix: 'x-ui',
    
        /**
        * 样式表路径默认前缀
        * @member {string} stylesheetPathPrefix
        * @memberof x.ui
        */
        stylesheetPathPrefix: '/resources/styles/x-ui/',
    
        packagesPathPrefix: null,
    
        /**
        * 通用 组件包默认名称空间
        * @namespace pkg
        * @memberof x.ui
        */
        pkg: {
    
            /**
            * 触摸组件包默认名称空间
            * @namespace touches
            * @memberof x.ui.pkg
            */
            touches: {},
    
            /**
            * 组件包根目录
            * @method dir
            * @memberof x.ui.pkg
            */
            dir: function() { return x.dir() + 'ui/pkg/'; }
        },
    
        /**
        * 样式名称空间
        * @namespace pkg
        * @memberof x.ui
        */
        styles: {
            dir: function() { return x.ui.stylesheetPathPrefix; }
        }
    };

    
    /**
    * @namespace debug
    * @memberof x
    * @description 基于 Console 对象的调试跟踪工具
    */
    x.debug = {
    
        // 相关链接
        // http://getfirebug.com/wiki/index.php/Console_API
        // https://developers.google.com/chrome-developer-tools/docs/console?hl=zh-CN#using_the_console_api
    
        /*#region 函数:log(object)*/
        /**
        * 记录普通的日志消息
        * @method log
        * @memberof x.debug
        * @param {object} object 对象
        */
        log: function(object)
        {
            // firebug
            if (!x.isUndefined(console))
            {
                console.log(object);
            }
        },
        /*#endregion*/
    
        /*#region 函数:warn(object)*/
        /**
        * 记录警告的日志消息
        * @method warn
        * @memberof x.debug
        * @param {object} object 对象
        */
        warn: function(object)
        {
            // firebug
            if (!x.isUndefined(console))
            {
                console.warn(object);
            }
        },
        /*#endregion*/
    
        /*#region 函数:error(object)*/
        /**
        * 记录错误的日志消息
        * @method error
        * @memberof x.debug
        * @param {object} object 对象
        */
        error: function(object)
        {
            // firebug
            if (!x.isUndefined(console))
            {
                console.error(object);
            }
        },
        /*#endregion*/
    
        /*#region 函数:assert(expression)*/
        /**
        * 断言
        * @method assert
        * @memberof x.debug
        * @param {string} expression 表达式
        */
        assert: function(expression)
        {
            // firebug
            if (!x.isUndefined(console))
            {
                console.assert(expression);
            }
        },
        /*#endregion*/
    
        /*#region 函数:time(name)*/
        /**
        * 计时器
        * @method time
        * @memberof x.debug
        * @param {string} name 计时器名称
        */
        time: function(name)
        {
            // firebug
            if (!x.isUndefined(console))
            {
                console.time(name);
            }
        },
        /*#endregion*/
    
        /*#region 函数:timeEnd(name)*/
        /**
        * 停止计时器
        * @method timeEnd
        * @memberof x.debug
        * @param {string} name 计时器名称
        */
        timeEnd: function(name)
        {
            // firebug
            if (!x.isUndefined(console))
            {
                console.timeEnd(name);
            }
        },
        /*#endregion*/
    
        /*#region 函数:timestamp()*/
        /**
        * 获取当前时间信息
        * @method timestamp
        * @memberof x.debug
        */
        timestamp: function()
        {
            // 显示时间格式
            var format = '{yyyy-MM-dd HH:mm:ss.fff}';
            // 当前时间戳
            var timestamp = new Date();
    
            return format.replace(/yyyy/, timestamp.getFullYear())
                         .replace(/MM/, (timestamp.getMonth() + 1) > 9 ? (timestamp.getMonth() + 1).toString() : '0' + (timestamp.getMonth() + 1))
                         .replace(/dd|DD/, timestamp.getDate() > 9 ? timestamp.getDate() : '0' + timestamp.getDate())
                         .replace(/hh|HH/, timestamp.getHours() > 9 ? timestamp.getHours() : '0' + timestamp.getHours())
                         .replace(/mm/, timestamp.getMinutes() > 9 ? timestamp.getMinutes() : '0' + timestamp.getMinutes())
                         .replace(/ss|SS/, timestamp.getSeconds() > 9 ? timestamp.getSeconds() : '0' + timestamp.getSeconds())
                         .replace(/fff/g, ((timestamp.getMilliseconds() > 99) ? timestamp.getMilliseconds() : (timestamp.getMilliseconds() > 9) ? '0' + timestamp.getMilliseconds() : '00' + timestamp.getMilliseconds()));
        }
        /*#endregion*/
    };

    
    /**
    * @namespace encoding
    * @memberof x
    * @description 编码
    */
    x.encoding = {
        /**
        * @namespace html
        * @memberof x.encoding
        * @description html 编码管理
        */
        html: {
            // http://www.w3.org/MarkUp/html-spec/html-spec_13.html
            dict: {
                '&': '&#32;',
                ' ': '&#38;',
                '<': '&#60;',
                '>': '&#62;',
                '"': '&#34;',
                '\'': '&#39;'
            },
    
            /*#region 函数:encode(text)*/
            /**
            * html 编码
            * @method encode
            * @memberof x.encoding.html
            * @param {string} text 文本信息
            * @example
            * // 输出格式 &#60;p&#62;hello&#60;/p&#62;
            * console.log(x.encoding.html.encode('<p>hello</p>'));
            */
            encode: function(text)
            {
                // 空值判断
                if (text.length === 0) { return ''; }
    
                text = x.string.stringify(text);
    
                return text.replace(/&(?![\w#]+;)|[<>"']/g, function(s)
                {
                    return x.encoding.html.dict[s];
                });
    
                //            var outString = text.replace(/&/g, '&amp;');
    
                //            outString = outString.replace(/</g, '&lt;');
                //            outString = outString.replace(/>/g, '&gt;');
                //            outString = outString.replace(/ /g, '&nbsp;');
                //            outString = outString.replace(/\'/g, '&#39;');
                //            outString = outString.replace(/\"/g, '&quot;');
    
                //            return outString;
            },
            /*#endregion*/
    
            /*#region 函数:decode(text)*/
            /**
            * html 解码
            * @method decode
            * @memberof x.encoding.html
            * @param {string} text 文本信息
            */
            decode: function(text)
            {
                // 空值判断
                if (text.length === 0) { return ''; }
    
                text = x.string.stringify(text);
    
                var outString = '';
    
                outString = text.replace(/&amp;/g, "&");
    
                outString = outString.replace(/&lt;/g, "<");
                outString = outString.replace(/&gt;/g, ">");
                outString = outString.replace(/&nbsp;/g, "  ");
                outString = outString.replace(/&#39;/g, "\'");
                outString = outString.replace(/&quot;/g, "\"");
    
                return outString;
            }
            /*#endregion*/
        },
    
        /**
        * @namespace unicode
        * @memberof x.encoding
        * @description unicode 编码管理
        */
        unicode: {
    
            // 注意
            // html 的 unicode 编码格式是&#888888;, javascript 的 unicode 编码格式\u000000
    
            /*#region 函数:encode(text)*/
            /**
            * unicode 编码
            * @method encode
            * @memberof x.encoding.unicode
            * @param {string} text 文本信息
            */
            encode: function(text, prefix)
            {
                if (text.length === 0) { return ''; }
    
                prefix = x.isUndefined(prefix) ? '\\u' : prefix;
    
                text = x.string.stringify(text);
    
                var outString = '';
    
                for (var i = 0; i < text.length; i++)
                {
                    var temp = text.charCodeAt(i).toString(16);
    
                    if (temp.length < 4)
                    {
                        while (temp.length < 4)
                        {
                            temp = '0'.concat(temp);
                        }
                    }
    
                    // temp = '\\u' + temp.slice(2, 4).concat(temp.slice(0, 2));
                    // temp = '\\u' + temp;
    
                    outString = outString.concat(prefix + temp);
                }
    
                return outString.toLowerCase();
            },
            /*#endregion*/
    
            /*#region 函数:decode(text)*/
            /**
            * unicode 解码
            * @method decode
            * @memberof x.encoding.unicode
            * @param {string} text 文本信息
            */
            decode: function(text, prefix)
            {
                if (text.length === 0) { return ''; }
    
                prefix = x.isUndefined(prefix) ? '\\u' : prefix;
    
                text = x.string.stringify(text);
    
                var outString = '';
    
                var list = text.match(/([\w]+)|(\\u([\w]{4}))/g);
    
                if (list != null)
                {
                    list.each(function(node, index)
                    {
                        if (node.indexOf(prefix) == 0)
                        {
                            outString += String.fromCharCode(parseInt(node.slice(2, 6), 16));
                        }
                        else
                        {
                            outString += node;
                        }
                    });
                }
    
                return outString;
            }
            /*#endregion*/
        }
    };

    
    /**
    * @namespace cookies
    * @memberof x
    * @description Cookies 管理
    */
    x.cookies = {
    
        /*#region 函数:query(name)*/
        /**
        * 根据 Cookie 名称查找相关的值
        * @method query
        * @memberof x.cookies
        * @param {string} name 名称
        */
        query: function(name)
        {
            var value = '';
            var search = name + '=';
    
            if (document.cookie.length > 0)
            {
                var offset = document.cookie.indexOf(search);
    
                if (offset != -1)
                {
                    offset += search.length;
    
                    var end = document.cookie.indexOf(';', offset);
    
                    if (end == -1)
                        end = document.cookie.length;
    
                    value = unescape(document.cookie.substring(offset, end));
                }
            }
    
            return value;
        },
        /*#endregion*/
    
        /*#region 函数:add(name, value, options)*/
        /**
        * 新增 Cookie 的值
        * @method add
        * @memberof x.cookies
        * @param {string} name 名称
        * @param {string} value 值
        * @param {object} [options] 选项<br />
        * 可选键值范围: 
        * <table class="param-options" >
        * <thead>
        * <tr><th>名称</th><th>类型</th><th class="last" >描述</th></tr>
        * </thead>
        * <tbody>
        * <tr><td class="name" >expire</td><td>string</td><td>过期时间</td></tr> 
        * <tr><td class="name" >path</td><td>string</td><td>所属的路径</td></tr>
        * <tr><td class="name" >domain</td><td>string</td><td>所属的域</td></tr>
        * </tbody>
        * </table>
        * @example
        * // 新增一条 Cookie 记录, 
        * // 名称为 CookieName1, 值为 CookieValue1
        * x.cookie.add('CookieName1', 'CookieValue1');
        * @example
        * // 新增一条 Cookie 记录, 
        * // 名称为 CookieName2, 值为 CookieValue2, 
        * // 过期时间为 2050-1-1 10:30:00 
        * x.cookie.add('cookieName2', 'cookieValue2', {'expire': new (2050, 1, 1, 10, 30, 00)});
        * @example
        * // 新增一条 Cookie 记录, 
        * // 名称为 CookieName3, 值为 CookieValue3, 
        * // 过期时间为 2050-1-1 10:30:00 , 所属路径为 /help/
        * x.cookies.add('cookieName3', 'cookieValue3', {'expire': new (2050,1,1,10,30,00), path: '/help/'});
        * @example
        * // 新增一条 Cookie 记录, 
        * // 名称为 CookieName4, 值为 CookieValue4, 
        * // 过期时间为 2050-1-1 10:30:00, 所属的域为 github.com
        * x.cookies.add('cookieName4', 'cookieValue4', {'expire': new (2050,1,1,10,30,00), path: '/', domain: 'github.com');
        */
        add: function(name, value, options)
        {
            options = x.ext({ path: '/' }, options || {});
    
            document.cookie = escape(name) + '=' + escape(value)
                + ((!options.expire) ? '' : ('; expires=' + options.expire.toGMTString()))
                + '; path=' + options.path
                + ((!options.domain) ? '' : ('; domain=' + options.domain));
        },
        /*#endregion*/
    
        /*#region 函数:remove(name, options)*/
        /**
        * 移除 Cookie 的值
        * @method remove
        * @memberof x.cookies
        * @param {string} name 名称
        * @param {object} [options] 选项<br />
        * 可选键值范围: 
        * <table class="param-options" >
        * <thead>
        * <tr><th>名称</th><th>类型</th><th class="last" >描述</th></tr>
        * </thead>
        * <tbody>
        * <tr><td class="name" >path</td><td>string</td><td>所属的路径</td></tr>
        * <tr><td class="name" >domain</td><td>string</td><td>所属的域</td></tr>
        * </tbody>
        * </table>
        * @example
        * // 移除一条 Cookie 记录, 名称为 CookieName1
        * x.cookies.remove('CookieName1');
        * @example
        * // 移除一条 Cookie 记录, 名称为 CookieName2
        * x.cookies.remove('CookieName2', {path: '/help/'});
        */
        remove: function(name, options)
        {
            options = x.ext({ path: '/' }, options || {});
    
            if (!!x.cookies.query(name))
            {
                document.cookie = escape(name) + '=; path=' + options.path
                    + '; expires=' + new Date(0).toGMTString()
                    + ((!options.domain) ? '' : ('; domain=' + options.domain));
            }
        }
        /*#endregion*/
    };

    
    /**
    * @namespace css
    * @memberof x
    * @description CSS
    */
    var css = x.css = {
        /**
        * 特殊关键字映射关系字典
        * @private
        */
        dict: {},
    
        /*#region 函数:style(selector)*/
        /**
        * 获取或设置元素对象的样式信息
        * @method style
        * @memberof x.css
        * @param {string} selector 选择器或者元素对象
        * @returns {CSSStyleDeclaration} {@like https://developer.mozilla.org/en-US/docs/Web/API/CSSStyleDeclaration|CSSStyleDeclaration}
        */
        style: function(selector)
        {
            var element = x.query(selector);
    
            if (arguments.length == 1)
            {
                return element.currentStyle || window.getComputedStyle(element, null);
            }
            else if (arguments.length == 2)
            {
                if (x.type(arguments[1]) == 'object')
                {
                    var options = arguments[1];
    
                    x.each(options, function(key, value)
                    {
                        element.style[x.camelCase(key)] = options[key];
                    });
                }
            }
            else if (arguments.length == 3)
            {
                if (x.type(arguments[1]) == 'string')
                {
                    element.style[arguments[1]] = arguments[2];
                }
            }
        },
        /*#endregion*/
    
        /*#region 函数:check(selector, className)*/
        /**
        * 检测元素对象的 className 是否存在
        * @method check
        * @memberof x.css
        * @param {string} selector 选择器或者元素对象
        * @param {string} className CSS类名称
        * @returns {bool}
        */
        check: function(selector, className)
        {
            var element = x.query(selector);
    
            if (element == null) return;
    
            /*
            var found=false;
            var buffer=o.className.split(' ');
    
            for(var i=0;i<buffer.length;i++){
            if(buffer[i]==className){found=true;}
            }
            return found;
            */
    
            var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
    
            return element.className.match(reg) == null ? false : true;
        },
        /*#endregion*/
    
        /*#region 函数:swap(selector, classNameA, classNameB)*/
        /**
        * 替换元素对象的 className
        * @method swap
        * @memberof x.css
        * @param {string} selector 选择器或者元素对象
        * @param {string} classNameA CSS类名称
        * @param {string} classNameB CSS类名称
        */
        swap: function(selector, classNameA, classNameB)
        {
            var element = x.query(selector);
    
            if (css.check(element, classNameA))
            {
                var buffer = element.className.split(' ');
    
                for (var i = 0; i < buffer.length; i++)
                {
                    buffer[i] = buffer[i].trim();
    
                    if (buffer[i] == classNameA) { buffer[i] = classNameB; }
                }
    
                element.className = buffer.join(' ');
            }
        },
        /*#endregion*/
    
        /*#region 函数:add(selector, className)*/
        /**
        * 添加元素对象的 className
        * @method add
        * @memberof x.css
        * @param {string} selector 选择器或者元素对象
        * @param {string} className CSS类名称
        */
        add: function(selector, className)
        {
            var element = x.query(selector);
    
            if (element == null) return;
    
            if (!css.check(element, className))
            {
                element.className += ' ' + className;
                element.className = element.className.trim();
            }
        },
        /*#endregion*/
    
        /*#region 函数:remove(selector, className)*/
        /**
        * 移除元素对象的 className
        * @method remove
        * @memberof x.css
        * @param {string} selector 选择器或者元素对象
        * @param {string} className CSS类名称
        */
        remove: function(selector, className)
        {
            var element = x.query(selector);
            
            if (element == null) return;
    
            if (css.check(element, className))
            {
                var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
    
                element.className = element.className.replace(reg, '');
                element.className = element.className.trim();
            }
        }
        /*#endregion*/
    };// -*- ecoding=utf-8 -*-
    
    /**
    * @namespace date
    * @memberof x
    * @description 日期和时间
    * @requires module:x
    */
    x.date = {
        /**
        * 当前时间对象
        * @method now
        * @memberof x.date
        */
        now: function()
        {
            return x.date.create();
        },
    
        /**
        * 创建时间对象
        * @method create
        * @memberof x.date
        * @param {object} timeValue 符合时间规则的日期, 数组, 字符串
        */
        create: function(timeValue)
        {
            return new x.date.newTime(timeValue);
        },
    
        /**
        * 时间间隔简写表
        * @method shortIntervals
        * @memberof x.date
        * @private
        */
        shortIntervals:
        {
            'y': 'year',
            'q': 'quarter',
            'M': 'month',
            'w': 'week',
            'd': 'day',
            'h': 'hour',
            'm': 'minute',
            's': 'second',
            'ms': 'msecond'
        },
    
        /**
        * 格式化时间间隔参数
        * @method formatInterval
        * @memberof x.date
        * @private
        */
        formatInterval: function(interval)
        {
            return x.date.shortIntervals[interval] || interval;
        },
    
        /**
        * 比较两个时间差异
        * @method diff
        * @memberof x.date
        */
        diff: function(begin, end, interval)
        {
            var timeBegin = new x.date.newTime(begin);
            var timeEnd = new x.date.newTime(end);
    
            return timeBegin.diff(x.date.formatInterval(interval), timeEnd);
        },
    
        /**
        * 比较两个时间差异
        * @method add
        * @memberof x.date
        */
        add: function(timeValue, interval, number)
        {
            var time = new x.date.newTime(timeValue);
    
            return time.add(x.date.formatInterval(interval), number);
        },
    
        /**
        * 时间格式化
        * @method format
        * @memberof x.date
        * @param {object} timeValue 符合时间规则的日期, 数组, 字符串
        * @param {string} formatValue 时间格式
        * @example
        * x.date.format('2000-01-01 00:00:00', 'yyyy/MM/dd hh:mm:ss');
        */
        format: function(timeValue, formatValue)
        {
            var time = x.date.create(timeValue);
    
            return time.toString(formatValue);
        },
    
        /**
        * 显示某个时间之前的格式
        * @method format
        * @memberof x.date
        * @param {object} timeValue 符合时间规则的日期, 数组, 字符串
        * @param {object} suffix 后缀配置
        * @example
        * x.date.ago('2000-01-01 00:00:00');
        * @example
        * x.date.ago('2000-01-01 00:00:00',{y});
        */
        ago: function(timeValue, suffix)
        {
            suffix = x.ext({
                minute: '分钟前',
                hour: '小时前',
                day: '天前'
            }, suffix);
    
            var time = x.date.create(timeValue);
            var now = x.date.create();
    
            if (time.diff('m', now) < 1)
            {
                return '1' + suffix.minute;
            }
            else if (time.diff('m', now) < 60)
            {
                return time.diff('m', now) + suffix.minute;
            }
            else if (time.diff('h', now) < 24)
            {
                return time.diff('h', now) + suffix.hour;
            }
            else if (time.diff('d', now) < 4)
            {
                return time.diff('d', now) + suffix.day;
            }
            else
            {
                return time.toString("yyyy-MM-dd HH:mm:ss");
            }
        },
    
        /**
        * 时间对象
        * @class Time 时间对象
        * @constructor newTime
        * @memberof x.date
        * @param {Date} timeValue 符合时间规则的Date对象, 数组对象, 字符串对象
        */
        newTime: function(timeValue)
        {
            var date = new Date();
    
            if (!x.isUndefined(timeValue))
            {
                if (x.type(timeValue) === 'date')
                {
                    // Date 对象
                    date = timeValue;
                }
                else if (x.isArray(timeValue))
                {
                    // Array 对象
                    var keys = timeValue
    
                    for (var i = 0; i < 6; i++)
                    {
                        keys[i] = isNaN(keys[i]) ? (i < 3 ? 1 : 0) : Number(keys[i]);
                    }
    
                    date = new Date(keys[0], Number(keys[1]) - 1, keys[2], keys[3], keys[4], keys[5]);
                }
                else if (/\/Date\((-?\d+)\)\//.test(timeValue))
                {
                    // .NET 日期对象
                    date = new Date(parseInt(timeValue.replace(/\/Date\((-?\d+)\)\//, '$1')));
                }
                else
                {
                    // 其他情况
                    var keys = timeValue.replace(/[-|:|\/| |年|月|日]/g, ',').split(',');
    
                    for (var i = 0; i < 6; i++)
                    {
                        keys[i] = isNaN(keys[i]) ? (i < 3 ? 1 : 0) : Number(keys[i]);
                    }
    
                    date = new Date(keys[0], Number(keys[1]) - 1, keys[2], keys[3], keys[4], keys[5]);
                }
            }
    
            var time = {
                year: date.getFullYear(),
                year2: date.getYear(),
                month: date.getMonth(),
                day: date.getDate(),
                hour: date.getHours(),
                minute: date.getMinutes(),
                second: date.getSeconds(),
                msecond: date.getMilliseconds(),
                weekDay: date.getDay(),
    
                /**
                * 比较与另一个时间对象的时间差
                * @method diff
                * @memberof x.date.newTime#
                * @param {string} interval 时间间隔
                * @param {Time} time 时间对象
                */
                diff: function(interval, time)
                {
                    var timeBegin = Number(this.toDate());
                    var timeEnd = Number(time.toDate());
    
                    interval = x.date.formatInterval(interval);
    
                    switch (interval)
                    {
                        case 'year': return time.year - this.year;
                        case 'quarter': return Math.ceil((((time.year - this.year) * 12) + (time.month - this.month)) / 3);
                        case 'month': return ((time.year - this.year) * 12) + (time.month - this.month);
                        case 'week': return parseInt((timeEnd - timeBegin) / (86400000 * 7));
                        case 'day': return parseInt((timeEnd - timeBegin) / 86400000);
                        case 'hour': return parseInt((timeEnd - timeBegin) / 3600000);
                        case 'minute': return parseInt((timeEnd - timeBegin) / 60000);
                        case 'second': return parseInt((timeEnd - timeBegin) / 1000);
                        case 'msecond': return parseInt((timeEnd - timeBegin));
                    }
                },
    
                /**
                * 时间对象的属性相加
                * @method add
                * @memberof x.date.newTime#
                * @param {string} interval 时间间隔
                * @param {number} number 数字
                */
                add: function(interval, number)
                {
                    var date = Number(this.toDate());
    
                    // 此毫秒表示的是需要创建的时间 和 GMT时间1970年1月1日 之间相差的毫秒数。
                    var ms = 0;
    
                    var monthMaxDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    
                    interval = x.date.formatInterval(interval);
    
                    switch (interval)
                    {
                        case 'year':
                            if ((this.year % 4 == 0 && ((this.year % 100 != 0) || (this.year % 400 == 0))) && this.month == 1 && this.day == 29
                                && !((this.year + number) % 4 == 0 && (((this.year + number) % 100 != 0) || ((this.year + number) % 400 == 0))))
                            {
                                // 闰年的二月二十九日并且目标年不为闰年
                                ms = Number(new Date(this.year + number, this.month, 28, this.hour, this.minute, this.second));
                            }
                            else
                            {
                                ms = Number(new Date(this.year + number, this.month, this.day, this.hour, this.minute, this.second));
                            }
                            break;
                        case 'quarter':
                            if ((this.year % 4 == 0 && ((this.year % 100 != 0) || (this.year % 400 == 0))) && this.month == 1 && this.day == 29
                                && !((this.year + parseInt((this.month + number * 3) / 12)) % 4 == 0 && (((this.year + parseInt((this.month + number * 3) / 12)) % 100 != 0) || ((this.year + parseInt((this.month + number * 3) / 12)) % 400 == 0))))
                            {
                                // 闰年的二月二十九日并且目标年不为闰年
                                ms = Number(new Date(this.year, (this.month + number * 3), 28, this.hour, this.minute, this.second));
                            }
                            else
                            {
                                if (this.day == monthMaxDays[this.month])
                                {
                                    // 月份最后一天的处理
                                    ms = Number(new Date(this.year, (this.month + number * 3), monthMaxDays[(this.month + number * 3) % 12], this.hour, this.minute, this.second));
                                }
                                else
                                {
                                    ms = Number(new Date(this.year, (this.month + number * 3), this.day, this.hour, this.minute, this.second));
                                }
                            }
                            break;
                        case 'month':
    
                            if ((this.year % 4 == 0 && ((this.year % 100 != 0) || (this.year % 400 == 0))) && this.month == 1 && this.day == 29
                                && !((this.year + parseInt((this.month + number) / 12)) % 4 == 0 && (((this.year + parseInt((this.month + number) / 12)) % 100 != 0) || ((this.year + parseInt((this.month + number) / 12)) % 400 == 0))))
                            {
                                // 闰年的二月二十九日并且目标年不为闰年
                                ms = Number(new Date(this.year, (this.month + number), 28, this.hour, this.minute, this.second));
                            }
                            else
                            {
                                if (this.day == monthMaxDays[this.month])
                                {
                                    // 月份最后一天的处理
                                    ms = Number(new Date(this.year, (this.month + number), monthMaxDays[(this.month + number) % 12], this.hour, this.minute, this.second));
                                }
                                else
                                {
                                    //ms = Number(this.toDate().setMonth(this.month + number));
                                    ms = Number(new Date(this.year, (this.month + number), this.day, this.hour, this.minute, this.second));
                                }
                            }
                            break;
                        case 'week':
                            ms = date + ((86400000 * 7) * number);
                            break;
                        case 'day':
                            ms = date + (86400000 * number);
                            break;
                        case 'hour':
                            ms = date + (3600000 * number);
                            break;
                        case 'minute':
                            ms = date + (60000 * number);
                            break;
                        case 'second':
                            ms = date + (1000 * number);
                            break;
                        case 'msecond':
                            ms = date + number;
                            break;
                    }
    
                    return x.date.create(new Date(ms));
                },
    
                /*
                * 取得日期数据信息
                * 参数 interval 表示数据类型
                * y 年 M月 d日 w星期 ww周 h时 n分 s秒
                */
                getDatePart: function(interval)
                {
                    var weekDays = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
    
                    interval = x.date.formatInterval(interval);
    
                    switch (interval)
                    {
                        case 'year':
                            return this.year;
                        case 'quarter':
                            return this.getQuarterOfYear();
                        case 'month':
                            return this.month;
                        case 'day':
                            return this.day;
                        case 'week':
                            return weekDays[this.weekDay];
                        case 'W':
                        case 'weekOfYear':
                            return this.getWeekOfYear();
                        case 'hour':
                            return this.hour;
                        case 'minute':
                            return this.minute;
                        case 'second':
                            return this.second;
                        default:
                            return 'Unkown Interval';
                    }
                },
    
                /**
                * 取得当前日期所在月的最大天数
                * @method getMaxDayOfMonth
                * @memberof x.date.newTime#
                */
                getMaxDayOfMonth: function()
                {
                    var date1 = x.date.create(this.toString('yyyy-MM-01'));
                    var date2 = x.date.create(this.add('month', 1).toString('yyyy-MM-01'));
    
                    return date1.diff('day', date2);
                },
    
                /**
                * 取得当前日期所在季度是一年中的第几季度
                * @method getQuarterOfYear
                * @memberof x.date.newTime#
                */
                getQuarterOfYear: function()
                {
                    return Math.ceil(this.month / 3);
                },
    
                /*
                * 取得当前日期是一年中的第几周
                */
                getWeekOfYear: function()
                {
                    var week = 0;
    
                    day = this.getDayOfYear();
    
                    // 判断是否为星期日
                    // 如果一年中的第一天不是星期日, 则减去相差的天数以最近的星期日开始计算
                    if (x.date.create(this.toString('yyyy-01-01')).weekDay > 0)
                    {
                        day = day - (7 - x.date.create(this.toString('yyyy-01-01')).weekDay);
                    }
    
                    if (day > 0)
                    {
                        week = Math.ceil(day / 7);
                    }
    
                    return week;
                },
    
                /*
                * 取得当前日期是一年中的第几天
                */
                getDayOfYear: function()
                {
                    var date1 = this.toDate();
                    var date2 = new Date(date1.getFullYear(), 0, 1);
    
                    return Math.ceil(Number(date1 - date2) / (24 * 60 * 60 * 1000)) + 1;
                },
    
                /*
                * 判断闰年
                */
                isLeapYear: function()
                {
                    // 闰年的计算方法：
                    // 公元纪年的年数可以被四整除，即为闰年；
                    // 被100整除而不能被400整除为平年；
                    // 被100整除也可被400整除的为闰年。
                    // 如2000年是闰年，而1900年不是。
                    return (this.year % 4 == 0 && ((this.year % 100 != 0) || (this.year % 400 == 0)));
                },
    
                /**
                * 转换为数组格式
                * @method toArray
                * @memberof x.date.newTime#
                * @returns {Array}
                */
                toArray: function()
                {
                    return [this.year, this.month, this.day, this.hour, this.minute, this.second, this.msecond];
                },
    
                /**
                * 转换为内置 Date 对象
                * @method toDate
                * @memberof x.date.newTime#
                * @returns {Date}
                */
                toDate: function()
                {
                    return new Date(this.year, this.month, this.day, this.hour, this.minute, this.second);
                },
    
                /**
                * 日期格式化
                * 格式
                * yyyy/yy 表示年份
                * MM 月份
                * w 星期
                * dd/d 日期
                * hh/h 时间
                * mm/m 分钟
                * ss/s 秒
                * @method toString
                * @memberof x.date.newTime#
                * @param {string} format 时间格式
                * @returns {string}
                */
                toString: function(format)
                {
                    var outString = x.isUndefined(format, 'yyyy-MM-dd HH:mm:ss');
    
                    var weekDays = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
    
                    return outString.replace(/yyyy|YYYY/g, this.year)
                                .replace(/yy|YY/g, x.paddingZero((this.year2 % 100), 2))
                                .replace(/MM/g, x.paddingZero((this.month + 1), 2))
                                .replace(/M/g, (this.month + 1))
    
                                .replace(/w|W/g, weekDays[this.weekDay])
    
                                .replace(/dd|DD/g, x.paddingZero(this.day, 2))
                                .replace(/d|D/g, this.day)
    
                                .replace(/hh|HH/g, x.paddingZero(this.hour, 2))
                                .replace(/h|H/g, this.hour)
    
                                .replace(/mm/g, x.paddingZero(this.minute, 2))
                                .replace(/m/g, this.minute)
    
                                .replace(/ss|SS/g, x.paddingZero(this.second, 2))
                                .replace(/s|S/g, this.second)
    
                                .replace(/fff/g, x.paddingZero(this.msecond, 3));
                }
            };
    
            return time;
        },
    
        /**
        * 时间间隔对象
        * @class TimeSpan
        * @constructor newTimeSpan
        * @memberof x.date
        * @param {number} timeSpanValue 符合时间规则的值(允许Date对象|数组对象|字符串对象)
        */
        newTimeSpan: function(timeSpanValue, format)
        {
            format = typeof (format) === 'undefined' ? 'second' : format;
    
            // 小时转化成秒
            if (format == 'day' || format == 'd')
            {
                timeSpanValue = timeSpanValue * 24 * 60 * 60;
            }
    
            // 小时转化成秒
            if (format == 'hour' || format == 'h')
            {
                timeSpanValue = timeSpanValue * 60 * 60;
            }
    
            // 分钟转化成秒
            if (format == 'minute' || format == 'm')
            {
                timeSpanValue = timeSpanValue * 60;
            }
    
            // 秒不需要转化
            if (format == 'second' || format == 's')
            {
                timeSpanValue = timeSpanValue * 1000;
            }
    
            var timeSpan = {
                // 时间间隔(单位:毫秒)
                timeSpanValue: timeSpanValue,
                // 天
                day: timeSpanValue / (24 * 60 * 60 * 1000),
                // 小时
                hour: timeSpanValue / (60 * 60 * 1000),
                // 分钟
                minute: timeSpanValue / (60 * 1000),
                // 秒
                second: timeSpanValue / 1000,
                // 毫秒
                millisecond: timeSpanValue % 1000,
    
                toString: function(format)
                {
                    var outString = '';
    
                    switch (format)
                    {
                        case 'MM天dd小时mm分钟ss秒fff毫秒':
                            outString = x.paddingZero(this.day, 2) + "天" + x.paddingZero(this.hour, 2) + "小时" + x.paddingZero(this.minute, 2) + "分钟" + x.paddingZero(this.second, 2) + "秒" + x.paddingZero(this.millisecond, 3) + "毫秒";
                            break;
                        case 'MM天dd小时mm分钟ss秒':
                            outString = x.paddingZero(this.day, 2) + "天" + x.paddingZero(this.hour, 2) + "小时" + x.paddingZero(this.minute, 2) + "分钟" + x.paddingZero(this.second, 2) + "秒";
                            break;
                        default:
                            outString = x.paddingZero(this.day, 2) + "天" + x.paddingZero(this.hour, 2) + "小时" + x.paddingZero(this.minute, 2) + "分钟" + x.paddingZero(this.second, 2) + "秒";
                            break;
                    }
    
                    return outString;
                }
            };
    
            return timeSpan;
        }
    };

    
    /**
    * @namespace expressions
    * @memberof x
    * @description 正则表达式管理
    */
    x.expressions = {
        /** 
        * 规则集合
        * @member {object} rules 
        * @memberof x.expressions
        * @example
        * // 返回邮箱地址的正则表达式
        * x.expressions.rules['email'];
        */
        rules: {
            // -----------------------------------------------------------------------------
            // 正则表达式全部符号解释
            // -----------------------------------------------------------------------------
            // \            将下一个字符标记为一个特殊字符、或一个原义字符、或一个 向后引用、或一个八进制转义符。
            //              例如，'n' 匹配字符 "n"。'\n' 匹配一个换行符。序列 '\\' 匹配 "\" 而 "\(" 则匹配 "("。
            // ^            匹配输入字符串的开始位置。如果设置了 RegExp 对象的 Multiline 属性，^ 也匹配 '\n' 或 '\r' 之后的位置。
            // $            匹配输入字符串的结束位置。如果设置了RegExp 对象的 Multiline 属性，$ 也匹配 '\n' 或 '\r' 之前的位置。
            // *            匹配前面的子表达式零次或多次。例如，zo* 能匹配 "z" 以及 "zoo"。* 等价于{0,}。
            // +            匹配前面的子表达式一次或多次。例如，'zo+' 能匹配 "zo" 以及 "zoo"，但不能匹配 "z"。+ 等价于 {1,}。
            // ?            匹配前面的子表达式零次或一次。例如，"do(es)?" 可以匹配 "do" 或 "does" 中的"do" 。? 等价于 {0,1}。
            // {n}          n 是一个非负整数。匹配确定的 n 次。例如，'o{2}' 不能匹配 "Bob" 中的 'o'，但是能匹配 "food" 中的两个 o。
            // {n,}         n 是一个非负整数。至少匹配n 次。例如，'o{2,}' 不能匹配 "Bob" 中的 'o'，但能匹配 "foooood" 中的所有 o。
            //              'o{1,}' 等价于 'o+'。'o{0,}' 则等价于 'o*'。
            // {n,m}        m 和 n 均为非负整数，其中n <= m。最少匹配 n 次且最多匹配 m 次。例如，"o{1,3}" 将匹配 "fooooood" 中的前三个 o。
            //              'o{0,1}' 等价于 'o?'。请注意在逗号和两个数之间不能有空格。
            // ?	        当该字符紧跟在任何一个其他限制符 (*, +, ?, {n}, {n,}, {n,m}) 后面时，匹配模式是非贪婪的。非贪婪模式尽可能少的匹配所搜索的字符串，而默认的贪婪模式则尽可能多的匹配所搜索的字符串。例如，对于字符串 "oooo"，'o+?' 将匹配单个 "o"，而 'o+' 将匹配所有 'o'。
            // .	        匹配除 "\n" 之外的任何单个字符。要匹配包括 '\n' 在内的任何字符，请使用象 '[.\n]' 的模式。
            // (pattern)	匹配 pattern 并获取这一匹配。所获取的匹配可以从产生的 Matches 集合得到，在VBScript 中使用 SubMatches 集合，在JScript 中则使用 $0…$9 属性。要匹配圆括号字符，请使用 '\(' 或 '\)'。
            // (?:pattern)	匹配 pattern 但不获取匹配结果，也就是说这是一个非获取匹配，不进行存储供以后使用。这在使用 "或" 字符 (|) 来组合一个模式的各个部分是很有用。例如， 'industr(?:y|ies) 就是一个比 'industry|industries' 更简略的表达式。
            // (?=pattern)	正向预查，在任何匹配 pattern 的字符串开始处匹配查找字符串。这是一个非获取匹配，也就是说，该匹配不需要获取供以后使用。例如，'Windows (?=95|98|NT|2000)' 能匹配 "Windows 2000" 中的 "Windows" ，但不能匹配 "Windows 3.1" 中的 "Windows"。预查不消耗字符，也就是说，在一个匹配发生后，在最后一次匹配之后立即开始下一次匹配的搜索，而不是从包含预查的字符之后开始。
            // (?!pattern)	负向预查，在任何不匹配 pattern 的字符串开始处匹配查找字符串。这是一个非获取匹配，也就是说，该匹配不需要获取供以后使用。例如'Windows (?!95|98|NT|2000)' 能匹配 "Windows 3.1" 中的 "Windows"，但不能匹配 "Windows 2000" 中的 "Windows"。预查不消耗字符，也就是说，在一个匹配发生后，在最后一次匹配之后立即开始下一次匹配的搜索，而不是从包含预查的字符之后开始
            // x|y	        匹配 x 或 y。例如，'z|food' 能匹配 "z" 或 "food"。'(z|f)ood' 则匹配 "zood" 或 "food"。
            // [xyz]	    字符集合。匹配所包含的任意一个字符。例如， '[abc]' 可以匹配 "plain" 中的 'a'。
            // [^xyz]	    负值字符集合。匹配未包含的任意字符。例如， '[^abc]' 可以匹配 "plain" 中的'p'。
            // [a-z]	    字符范围。匹配指定范围内的任意字符。例如，'[a-z]' 可以匹配 'a' 到 'z' 范围内的任意小写字母字符。
            // [^a-z]	    负值字符范围。匹配任何不在指定范围内的任意字符。例如，'[^a-z]' 可以匹配任何不在 'a' 到 'z' 范围内的任意字符。
            // \b	        匹配一个单词边界，也就是指单词和空格间的位置。例如， 'er\b' 可以匹配"never" 中的 'er'，但不能匹配 "verb" 中的 'er'。
            // \B	        匹配非单词边界。'er\B' 能匹配 "verb" 中的 'er'，但不能匹配 "never" 中的 'er'。
            // \cx	        匹配由 x 指明的控制字符。例如， \cM 匹配一个 Control-M 或回车符。x 的值必须为 A-Z 或 a-z 之一。否则，将 c 视为一个原义的 'c' 字符。
            // \d	        匹配一个数字字符。等价于 [0-9]。
            // \D	        匹配一个非数字字符。等价于 [^0-9]。
            // \f	        匹配一个换页符。等价于 \x0c 和 \cL。
            // \n	        匹配一个换行符。等价于 \x0a 和 \cJ。
            // \r	        匹配一个回车符。等价于 \x0d 和 \cM。
            // \s	        匹配任何空白字符，包括空格、制表符、换页符等等。等价于 [ \f\n\r\t\v]。
            // \S	        匹配任何非空白字符。等价于 [^ \f\n\r\t\v]。
            // \t	        匹配一个制表符。等价于 \x09 和 \cI。
            // \v	        匹配一个垂直制表符。等价于 \x0b 和 \cK。
            // \w	        匹配包括下划线的任何单词字符。等价于'[A-Za-z0-9_]'。
            // \W	        匹配任何非单词字符。等价于 '[^A-Za-z0-9_]'。
            // \xn	        匹配 n，其中 n 为十六进制转义值。十六进制转义值必须为确定的两个数字长。例如，'\x41' 匹配 "A"。'\x041' 则等价于 '\x04' & "1"。正则表达式中可以使用 ASCII 编码。.
            // \num	        匹配 num，其中 num 是一个正整数。对所获取的匹配的引用。例如，'(.)\1' 匹配两个连续的相同字符。
            // \n	        标识一个八进制转义值或一个向后引用。如果 \n 之前至少 n 个获取的子表达式，则 n 为向后引用。否则，如果 n 为八进制数字 (0-7)，则 n 为一个八进制转义值。
            // \nm	        标识一个八进制转义值或一个向后引用。如果 \nm 之前至少有 nm 个获得子表达式，则 nm 为向后引用。如果 \nm 之前至少有 n 个获取，则 n 为一个后跟文字 m 的向后引用。如果前面的条件都不满足，若 n 和 m 均为八进制数字 (0-7)，则 \nm 将匹配八进制转义值 nm。
            // \nml	        如果 n 为八进制数字 (0-3)，且 m 和 l 均为八进制数字 (0-7)，则匹配八进制转义值 nml。
            // \un	        匹配 n，其中 n 是一个用四个十六进制数字表示的 Unicode 字符。例如， \u00A9 匹配版权符号 (?)。
            // -----------------------------------------------------------------------------
            // 正则表达式的标准写法
            // -----------------------------------------------------------------------------
            // regexp = new RegExp(pattern[, flag]);
            // pattern  : 模板的用法是关键，也是本章的主要内容。
            // flag     : "i"(ignore)、"g"(global)、"m"(multiline)的组合
            //            i-忽略大小写，g-反复检索，m-多行检索     flag中没有g时，返回字符串，有g时返回字符串数组
    
            // 字符两侧空格 
            // \uFEFF 表示 BOM(Byte Order Mark) 即字节序标记 相关链接:http://zh.wikipedia.org/wiki/字节顺序记号
            // \xA0   表示 NBSP = CHAR(160) 即字节序标记 相关链接:http://zh.wikipedia.org/wiki/不换行空格
            'trim': /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,
            // 日期
            'date': /((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))/g,
            // 链接地址
            //        var re = '^((https|http|ftp|rtsp|mms)?://)'
            //            + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //ftp的user@  
            //            + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184  
            //            + "|" // 允许IP和DOMAIN（域名） 
            //            + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.  
            //            + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名  
            //            + "[a-z]{2,6})" // first level domain- .com or .museum  
            //            + "(:[0-9]{1,4})?" // 端口- :80  
            //            + "((/?)|" // a slash isn't required if there is no file name  
            //            + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
            'url': "^((https|http|ftp|rtsp|mms)?://)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\.[a-z]{2,6})(:[0-9]{1,4})?((/?)|(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$",
            // 电话号码
            'telephone': /(^\d+$)|((^\d+)([\d|\-]+)((\d+)$))|((^\+)([\d|\-]+)((\d+)$))/g,
            // 非电话号码
            'non-telephone': /[^\d\-\+]/g,
            // 电子邮箱
            'email': /^\w+((-\w+)|(\_\w+)|(\'\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/g,
            // QQ号
            'qq': /^\w+((-\w+)|(\_\w+)|(\'\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/g,
            // 数字
            'number': /(^-?\d+$)|(^-?\d+[\.?]\d+$)/g,
            // 非数字 
            'non-number': /[^\d\.\-]/g,
            // 整数
            'integer': /^-?\d+$/g,
            // 正整数
            'positive-integer': /^\d+$/g,
            // 非整数 
            'non-integer': /[^\d\-]/g,
            // 安全字符
            'safeText': /A-Za-z0-9_\-/g,
            // 非安全字符
            'non-safeText': /[^A-Za-z0-9_\-]/g,
            // 安全文件扩展名
            'fileExt': 'jpg,gif,jpeg,png,bmp,psd,sit,tif,tiff,eps,png,ai,qxd,pdf,cdr,zip,rar',
            // 其他规则
            'en-us': {
                // 美国邮编规则
                'zipcode': /^\d{5}-\d{4}$|^\d{5}$/g
            },
            'zh-cn': {
                'identityCard': /(^\d{15}$)|(^\d{18}$)|(^\d{17}[X|x]$)/g,
                // 中国邮编规则 
                'zipcode': /^\d{6}$/g
            }
        },
    
        /*#region 函数:match(options)*/
        /**
        * 匹配
        */
        match: function(options)
        {
            // 文本信息
            var text = String(options.text);
            // 忽略大小写
            var ignoreCase = options.ignoreCase;
            // 规则名称
            var regexpName = options.regexpName;
            // 规则
            var regexp = typeof (options.regexp) === 'undefined' ? undefined : new RegExp(options.regexp);
    
            if (ignoreCase === 1)
            {
                text = text.toLowerCase();
            }
    
            // 如果没有填写规则，并且填写了内置规则名称，则使用内置规则。
            if (typeof (regexp) === 'undefined' && typeof (regexpName) !== 'undefined')
            {
                regexp = x.expressions.rules[regexpName];
            }
    
            return text.match(regexp);
        },
        /*#endregion*/
    
        /*#region 函数:exists(options)*/
        /**
        * 利用正则表达式验证字符串规则
        * @method exists
        * @memberof x.expressions
        * @param {object} options 选项信息
        * @example
        * // result = false;
        * var result = x.expressions.exists({
        *   text:'12345a',
        *   regexpName: 'number',
        *   ignoreCase: ture
        * });
        *
        * @example
        * // result = false;
        * var result = x.expressions.exists({
        *   text:'12345a',
        *   regexp: /^\d+$/g,
        *   ignoreCase: ture
        * });
        */
        exists: function(options)
        {
            var text = String(options.text);
            // 忽略大小写
            var ignoreCase = options.ignoreCase;
            // 规则名称
            var regexpName = options.regexpName;
            // 规则
            var regexp = typeof (options.regexp) === 'undefined' ? undefined : new RegExp(options.regexp);
    
            if (ignoreCase)
            {
                text = text.toLowerCase();
            }
    
            // 如果没有填写规则，并且填写了内置规则名称，则使用内置规则。
            if (typeof (regexp) === 'undefined' && typeof (regexpName) !== 'undefined')
            {
                regexp = x.expressions.rules[regexpName];
            }
    
            return text.exists(regexp);
        },
        /*#endregion*/
    
        /*#region 函数:isFileExt(path, allowFileExt)*/
        /**
        * 验证文件的扩展名.
        * @method isFileExt
        * @memberof x.expressions
        * @param {string} path 文件路径
        * @param {string} allowFileExt 允许的扩展名, 多个以半角逗号隔开
        */
        isFileExt: function(path, allowFileExt)
        {
            var result = false;
    
            var ext = path.substr(path.lastIndexOf('.'), path.length - path.lastIndexOf('.'));
    
            var extValue = ((allowFileExt) ? allowFileExt : x.expressions.rules['fileExt']);
    
            ext = ext.replace('.', '');
    
            if (extValue.indexOf(',') != -1)
            {
                var list = extValue.split(',');
    
                for (var i = 0; i < list.length; i++)
                {
                    if (ext.toLowerCase() == list[i])
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                if (ext.toLowerCase() == extValue)
                {
                    result = true;
                }
            }
    
            return result;
        },
        /*#endregion*/
    
        /*#region 函数:isUrl(text)*/
        /**
        * 验证URL地址格式
        * @method isUrl
        * @memberof x.expressions
        * @param {string} text 文本信息
        */
        isUrl: function(text)
        {
            return text.toLowerCase().exists(x.expressions.rules['url']);
        },
        /*#endregion*/
    
        /*#region 函数:isEmail(text)*/
        /*
        * 验证Email地址格式
        * @method isEmail
        * @memberof x.expressions
        * @param {string} text 文本信息
        */
        isEmail: function(text)
        {
            return text.toLowerCase().exists(x.expressions.rules['email']);
        },
        /*#endregion*/
    
        /*#region 函数:isZipcode(text, nature))*/
        /*
        * 验证邮编
        * @method isZipcode
        * @memberof x.expressions
        * @param {string} text 文本信息
        * @param {string} nature 区域信息
        */
        isZipcode: function(text, nature)
        {
            nature = x.formatNature(nature);
    
            return text.exists(x.expressions.rules[nature]['zipcode']);
        },
        /*#endregion*/
    
        /*#region 函数:isSafeText(text)*/
        /**
        * 验证输入的字符串是否为安全字符, 即只允许字母、数字、下滑线。
        * @method isSafeText
        * @memberof x.expressions
        * @param {string} text 文本信息
        */
        isSafeText: function(text)
        {
            return text.exists(x.expressions.rules['safeText']);
        },
        /*#endregion*/
    
        /*#region 函数:formatTelephone(text)*/
        /**
        * 格式化输入的输入的文本为电话号码.
        * @method formatTelephone
        * @memberof x.expressions
        * @param {string} text 文本信息
        */
        formatTelephone: function(text)
        {
            return text.replace(x.expressions.rules['non-telephone'], '');
        },
        /*#endregion*/
    
        /*#region 函数:formatInteger(value, removePaddingZero)*/
        /**
        * 格式化输入的输入的文本为整数.
        * @method formatInteger
        * @memberof x.expressions
        * @param {string} value 文本信息
        * @param {bool} [removePaddingZero] 移除两侧多余的零
        * @example
        * var value = '12345a';
        * // return value = '12345'
        * value = x.expressions.formatInteger(value);
        * @example
        * var value = '012345';
        * // return value = '12345'
        * value = x.expressions.formatInteger(value, true);
        */
        formatInteger: function(value, removePaddingZero)
        {
            // number : ^\d
            value = String(value).replace(x.expressions.rules['non-integer'], '');
    
            if (x.string.trim(value) === '')
            {
                value = '0';
            }
    
            // 去除两侧多余的零
            if (removePaddingZero)
            {
                value = parseInt(value, 10);
            }
    
            return value;
        },
        /*#endregion*/
    
        /*#region 函数:formatNumber(value, removePaddingZero)*/
        /**
        * 格式化输入的输入的文本为数字.
        * @method formatInteger
        * @memberof x.expressions
        * @param {string} value 文本信息
        * @param {bool} [removePaddingZero] 移除两侧多余的零
        * @example
        * var value = '12345.00a';
        * // return value = '12345'
        * value = x.expressions.formatInteger(value);
        * @example
        * var value = '012345.00';
        * // return value = '12345'
        * value = x.expressions.formatInteger(value, true);
        */
        formatNumber: function(value, removePaddingZero)
        {
            value = String(value).replace(x.expressions.rules['non-number'], '');
    
            // 检测空字符串
            value = (value.trim() === '') ? '0' : value;
    
            // 去除两侧多余的零
            if (removePaddingZero)
            {
                value = parseFloat(value, 10);
            }
    
            return value;
        },
        /*#endregion*/
    
        /*#region 函数:formatNumberRound2(value, removePaddingZero)*/
        /**
        * 格式化输入的文本统一为保留小数点后面两位的数字。
        * @method formatNumberRound2
        * @memberof x.expressions
        * @param {string} value 文本信息
        * @param {bool} [removePaddingZero] 移除两侧多余的零
        * @example
        * var value = '12345';
        * // return value = '12345.00'
        * value = x.expressions.formatNumberRound2(value);
        */
        formatNumberRound2: function(value, removePaddingZero)
        {
            if (typeof (removePaddingZero) === 'undefined')
            {
                removePaddingZero = 1;
            }
    
            var text = '' + Math.round(x.expressions.formatNumber(value) * 100) / 100;
    
            var index = text.indexOf('.');
    
            if (index < 0)
            {
                return text + '.00';
            }
    
            var text = text.substring(0, index + 1) + text.substring(index + 1, index + 3);
    
            if (index + 2 == text.length)
            {
                text += '0';
            }
    
            // 去除两侧多余的零
            if (removePaddingZero)
            {
                value = parseFloat(text);
            }
    
            return value;
        },
        /*#endregion*/
    
        /*#region 函数:formatSafeText(text)*/
        /**
        * 格式化输入的文本为安全字符(常用于登录名和拼音字母的检测)
        * @method formatSafeText
        * @memberof x.expressions
        * @param {string} text 文本信息
        * @example
        * var text = 'abcd-$1234';
        * // return value = 'abcd1234'
        * text = x.expressions.formatSafeText(text);
        */
        formatSafeText: function(text)
        {
            return text.replace(x.expressions.rules['non-safeText'], '');
        }
        /*#endregion*/
    };

    
    /**
    * @namespace dom
    * @memberof x
    * @description 页面元素管理
    */
    var dom = x.dom = function(selector, context)
    {
        return new dom.fn.init(selector, context);
    };
    
    dom.fn = dom.prototype;
    
    dom.fn.init = function()
    {
        var that = this;
    
        this.results = x.queryAll.apply(x.queryAll, arguments);
    
        x.each(this.results, function(index, node)
        {
            that[index] = node;
        });
    
        return this;
    };
    
    dom.fn.init.prototype = dom.fn;
    
    // 扩展对象的方法
    dom = x.ext(dom, {
    
        /*#region 函数:query(selector)*/
        /**
        * 精确查询单个表单元素，返回一个jQuery对象。
        * @method query
        * @memberof dom
        * @param {string} selector 选择表达式
        */
        query: function(selector)
        {
            // 默认根据id查找元素
            if (selector.indexOf('#') == -1 && selector.indexOf('.') == -1 && selector.indexOf(' ') == -1) { selector = '[id="' + selector + '"]'; }
    
            var result = x.query(selector);
    
            // $(null) 会返回 整个文档对象，所以定义一个特殊 __null__ 变量替代空值。
            return result === null ? $('#__null__') : $(result);
        },
        /*#endregion*/
    
        nodes: function(html)
        {
            var list = [];
    
            var tmp = document.createElement('div');
    
            tmp.innerHTML = html;
    
            for (var i = 0; i < tmp.childNodes.length; i++)
            {
                list[list.length] = tmp.childNodes[i].cloneNode(true);
            }
    
            return list;
        },
    
        /*#region 函数:on(target, type, listener, useCapture)*/
        /**
        * 添加事件监听器 x.event.add 的别名
        * @method add
        * @memberof x
        * @param {string} target 监听对象
        * @param {string} type 监听事件
        * @param {string} listener 处理函数
        * @param {string} [useCapture] 监听顺序方式
        */
        on: function(target, type, listener, useCapture)
        {
            return x.event.add(target, type, listener, useCapture);
        },
        /*#endregion*/
    
        /*#region 函数:off(target, type, listener, useCapture)*/
        /**
        * 移除事件监听器 x.event.remove 的别名
        * @method add
        * @memberof x
        * @param {string} target 监听对象
        * @param {string} type 监听事件
        * @param {string} listener 处理函数
        * @param {string} [useCapture] 监听顺序方式
        */
        off: function(target, type, listener, useCapture)
        {
            return x.event.add(target, type, listener, useCapture);
        },
        /*#endregion*/
    
        /*#region 函数:attr(id, name, value)*/
        /**
        * 获取或设置元素的属性信息
        * @method attr
        * @memberof dom
        * @param {string} id 元素 Id
        */
        attr: function(id, name, value)
        {
            var node = null;
    
            if (x.type(arguments[0]).indexOf('html') == 0)
            {
                // Html 元素类型 直接返回
                node = arguments[0];
            }
            else if (x.type(arguments[0]) == 'string')
            {
                node = document.getElementById(id);
            }
    
            if (node == null) { return null; }
    
            if (x.isUndefined(value))
            {
                return node.getAttribute(name);
            }
            else
            {
                node.setAttribute(name, value);
            }
        },
        /*#endregion*/
    
        /*#region 函数:options(id)*/
        /**
        * 获取元素的选项配置信息
        * @method options
        * @memberof dom
        * @param {string} id 元素 Id
        */
        options: function(id)
        {
            var value = dom.attr(id, 'x-dom-options');
    
            return (x.isUndefined(value) || value == null || value == '') ? {} : x.toJSON(value);
        },
        /*#endregion*/
    
        // 追加 html 标记
        append: function(element, html)
        {
            var nodes = dom.nodes(html);
    
            for (var i = 0; i < nodes.length; i++)
            {
                element.appendChild(nodes[i]);
            }
    
            return element;
        },
    
        // 创建包裹层
        wrap: function(element, html)
        {
            var tmp = document.createElement('div');
    
            tmp.innerHTML = html;
            tmp = tmp.children[0];
    
            var _element = element.cloneNode(true);
    
            tmp.appendChild(_element);
            element.parentNode.replaceChild(tmp, element);
    
            return tmp;
        },
    
        before: function(element, html)
        {
            var nodes = dom.nodes(html);
    
            for (var i = 0; i < nodes.length; i++)
            {
                element.parentNode.insertBefore(nodes[i], element);
            }
    
            return element;
        },
    
        after: function(element, html)
        {
            var nodes = dom.nodes(html);
    
            for (var i = 0; i < nodes.length; i++)
            {
                element.parentNode.insertBefore(nodes[i], element.nextSibling);
            }
    
            return element;
        },
    
        /*#region 函数:swap(options)*/
        /**
        * 交换控件的数据
        * @method swap
        * @memberof dom
        * @param {object} options 选项
        */
        swap: function(options)
        {
            var fromInput = dom.query(options.from);
            var toInput = dom.query(options.to);
    
            x.each(options.attributes, function(index, node)
            {
                if (fromInput.attr(node))
                {
                    toInput.attr(node, fromInput.attr(node));
    
                    fromInput.removeAttr(node);
                }
            });
        },
        /*#endregion*/
    
        /*#region 函数:fixed(selector, pointX, pointY)*/
        /**
        * 设置元素的位置
        * @method fixed
        * @memberof dom
        * @param {string} selector 选择表达式
        * @param {number} pointX X坐标
        * @param {number} pointY Y坐标
        */
        fixed: function(selector, pointX, pointY)
        {
            dom.css(selector, {
                'position': 'fixed',
                'left': pointX + 'px',
                'top': pointY + 'px'
            });
        },
        /*#endregion*/
    
        /*#region 函数:setOpacity(selector, value)*/
        /**
        * 设置容器的透明度
        * @method setOpacity
        * @memberof dom
        * @param {string} selector 选择表达式
        * @param {number} value 透明度范围(0.00 ~ 1.00)
        */
        setOpacity: function(selector, value)
        {
            var element = x.query(selector);
    
            if (x.browser.ie && element.style.filter)
            {
                // IE
                element.style.filter = 'alpha(opacity:' + value + ')';
            }
            else
            {
                //其他浏览器
                element.style.opacity = value / 100;
            }
        },
        /*#endregion*/
    
        utils: {},
    
        hooks: {},
    
        features: {
            // 默认配置信息
            defaults: {
                // 特性属性名称
                scope: 'input,textarea,div,span',
                // 脚本文件夹位置
                featureScriptFilePath: '',
                // 特性属性名称
                featureAttributeName: 'x-dom-feature',
                // 特性是否已加载标识属性名称
                featureLoadedAttributeName: 'x-dom-feature-loaded',
                // 监听函数, 参数 element 页面元素
                listen: null
            },
    
            /**
            * 绑定客户端控件
            */
            bind: function(options)
            {
                options = x.ext(dom.features.defaults, options || {});
    
                if (x.isUndefined || options.featureScriptPath == '')
                {
                    options.featureScriptPath = x.dir() + 'dom/features/';
                }
    
                var tags = options.scope.split(',');
    
                x.each(tags, function(index, node)
                {
                    var list = document.getElementsByTagName(node);
    
                    for (var i = 0; i < list.length; i++)
                    {
                        if (x.isFunction(options.listen))
                        {
                            options.listen(list[i]);
                        }
    
                        /*
                        try
                        {
                        if ($(list[i]).hasClass('custom-forms-data-required') || $(list[i]).hasClass('custom-forms-data-regexp'))
                        {
                        if (options.tooltip)
                        {
                        // x.ui.tooltip.newWarnTooltip({ element: list[i].id, hide: 1 });
                        }
                        }
                        }
                        catch (ex)
                        {
                        x.debug.error(ex);
                        }
                        */
                        try
                        {
                            if (x.isUndefined(list[i].id) || list[i].id === '')
                            {
                                continue;
                            }
    
                            var feature = dom('#' + list[i].id).attr(options.featureAttributeName);
    
                            if (feature != null && dom('#' + list[i].id).attr(options.featureLoadedAttributeName) != '1')
                            {
                                feature = x.camelCase(feature);
    
                                x.require({
                                    id: 'x-dom-feature-' + feature + '-script',
                                    path: options.featureScriptPath + 'x.dom.features.' + feature + '.js',
                                    data: { target: list[i], feature: feature },
                                    callback: function(context)
                                    {
                                        // x.debug.log('feature:' + feature + ',' + response.data.feature);
                                        var data = context.data;
    
                                        // 加载完毕后, 加个 featureLoaded 标识, 避免重复加载效果.
                                        dom('#' + data.target.id).attr(options.featureLoadedAttributeName, '1');
    
                                        if (x.isUndefined(dom.features[data.feature]))
                                        {
                                            x.debug.error('x.dom.features.bind() 异常:系统加载表单元素特性【' + data.feature + '】失败，请检查相关配置。');
                                        }
    
                                        dom.features[data.feature].bind(data.target.id);
                                    }
                                });
    
                                /*
                                // 加载完毕后, 加个 featureLoaded 标识, 避免重复加载效果.
                                dom.query(list[i].id).attr(options.featureLoadedAttributeName, '1');
    
                                if (x.isUndefined(dom.features[feature]))
                                {
                                x.debug.error('dom.features.bind() 异常:系统加载表单元素特性【' + feature + '】失败，请检查相关配置。');
                                continue;
                                }
    
                                dom.features[feature].bind(list[i].id);
                                */
                            }
                        }
                        catch (ex)
                        {
                            x.debug.error(ex)
                        }
                    }
                });
            }
        }
    });
    
    // DOM 扩展设置类方法
    x.each(['on', 'off', 'append', 'before', 'after', 'wrap'], function(index, name)
    {
        dom.fn[name] = function()
        {
            for (var i = 0; i < this.results.length; i++)
            {
                var args = Array.prototype.slice.call(arguments).slice(0);
    
                args.unshift(this.results[i]);
    
                dom[name].apply(this, args);
            }
    
            return this;
        };
    });
    
    // DOM 扩展获取类方法
    x.each(['attr', 'options'], function(index, name)
    {
        dom.fn[name] = function()
        {
            if (this.results.length > 0)
            {
                var args = Array.prototype.slice.call(arguments).slice(0);
    
                args.unshift(this.results[0]);
    
                return dom[name].apply(this, args);
            }
        };
    });
    
    // ready 准备函数
    
    x.ext(dom, {
        // Is the DOM ready to be used? Set to true once it occurs.
        isReady: false,
    
        // 准备事件列表
        readyList: [],
    
        // DOM 加载完毕后执行
        ready: function()
        {
            // 简化调用方法 x.dom(document).ready(fn) => x.dom.ready(fn)
            if (x.isFunction(arguments[0]))
            {
                return dom(document).ready(arguments[0]);
            }
    
            // Abort if there are pending holds or we're already ready
            if (dom.isReady)
            {
                return;
            }
    
            // Remember that the DOM is ready
            dom.isReady = true;
    
            // If there are functions bound, to execute
            if (dom.readyList)
            {
                // Execute all of them
                x.each(dom.readyList, function()
                {
                    this.call(document);
                });
    
                // Reset the list of functions
                dom.readyList = null;
            }
        }
    });
    
    dom.fn.ready = function(fn)
    {
        // Attach the listeners
        bindReady();
    
        if (dom.isReady)
        {
            // If the DOM is already ready
            // Execute the function immediately
            // fn.call(document, dom);
            completed();
        }
        else
        {
            // Otherwise, remember the function for later
            // Add the function to the wait list
            dom.readyList.push(fn);
        }
        return this;
    };
    
    // 执行准备函数并且移除事件
    function completed()
    {
        // 支持旧版的 IE : readyState === "complete"
        if (document.addEventListener || event.type === "load" || document.readyState === "complete")
        {
            if (document.addEventListener)
            {
                document.removeEventListener("DOMContentLoaded", completed, false);
                window.removeEventListener("load", completed, false);
            }
            else
            {
                document.detachEvent("onreadystatechange", completed);
                window.detachEvent("onload", completed);
            }
    
            dom.ready();
        }
    }
    
    // 界限
    var readyBound = false;
    
    function bindReady()
    {
        if (readyBound) return;
    
        readyBound = true;
    
        if (document.readyState === "complete")
        {
            // 处理异步的文档加载情况, 允许直接执行函数
            setTimeout(dom.ready);
        }
        else if (document.addEventListener)
        {
            // 支持 DOMContentLoaded 的标准浏览器
    
            // Use the handy event callback
            document.addEventListener("DOMContentLoaded", completed, false);
    
            // A fallback to window.onload, that will always work
            window.addEventListener("load", completed, false);
        }
        else if (document.attachEvent)
        {
            // If IE event model is used
    
            // ensure firing before onload,
            // maybe late but safe also for iframes
            // Ensure firing before onload, maybe late but safe also for iframes
            document.attachEvent("onreadystatechange", completed);
    
            window.attachEvent("onload", completed);
    
            // If IE and not an iframe
            // continually check to see if the document is ready
            if (document.documentElement.doScroll && window == window.top) (function()
            {
                if (dom.isReady) return;
    
                try
                {
                    // If IE is used, use the trick by Diego Perini
                    // http://javascript.nwbox.com/IEContentLoaded/
                    document.documentElement.doScroll("left");
                }
                catch (error)
                {
                    setTimeout(arguments.callee, 0);
                    return;
                }
    
                // and execute any waiting functions
                dom.ready();
            })();
        }
    }// -*- ecoding=utf-8 -*-
    
    /**
    * @namespace data
    * @memberof x.dom
    * @description 页面元素数据管理
    */
    dom.data = {
        // 默认配置信息
        defaults: {
            // 存储格式的类型
            storageType: 'normal',
            // 数据类型属性名称
            dataTypeAttributeName: 'x-dom-data-type',
            // 数据必填项验证属性名称
            dataRequiredAttributeName: 'x-dom-data-required',
            // 数据必填项验证失败提示信息证属性名称
            dataRequiredWarningAttributeName: 'x-dom-data-required-warning',
            // 数据正则表达式规则验证属性名称
            dataRegexpAttributeName: 'x-dom-data-regexp',
            // 数据正则表达式规则验证属性名称
            dataRegexpNameAttributeName: 'x-dom-data-regexp-name',
            // 数据正则表达式规则验证失败提示信息证属性名称
            dataRegexpWarningAttributeName: 'x-dom-data-regexp-warning'
        },
    
        /*#region 函数:bindInputData(options)*/
        /*
        * 绊定控件的数据
        */
        bindInputData: function(options)
        {
            // options.inputName ,options multiSelection
            var input = x.dom(options.inputName);
    
            if ('[contacts],[corporation],[project]'.indexOf(options.featureName) > -1)
            {
                // 根据data标签的数据内容设置隐藏值和文本信息
                var data = input.attr('x-dom-data');
    
                if (typeof (data) !== 'undefined' && data.indexOf('#') > -1)
                {
                    var selectedValue = '';
                    var selectedText = '';
    
                    var list = x.string.trim(data, ',').split(',');
    
                    for (var i = 0; i < list.length; i++)
                    {
                        selectedValue += list[i].split('#')[1] + ',';
                        selectedText += list[i].split('#')[2] + ';';
    
                        // 单选时,只取data第一个值
                        if (options.multiSelection === 0) { break; }
                    }
    
                    selectedValue = x.string.rtrim(selectedValue, ',');
                    selectedText = x.string.rtrim(selectedText, ';');
    
                    if (options.multiSelection === 1)
                    {
                        // 多选
                        input.val(data);
                    }
                    else
                    {
                        // 单选
                        input.val(selectedValue);
                    }
    
                    input.attr('selectedText', selectedText);
                }
            }
        },
        /*#endregion*/
    
        /*#region 函数:check(options)*/
        /*
        * 验证客户端数据
        */
        check: function(options)
        {
            // 设置默认选项参数
            options = x.ext({
                // 提示工具条
                tooltip: 0,
                // 提示框
                alert: 1
            }, options || {});
    
            var warning = '';
    
            var list = x.dom('*');
    
            x.each(list, function(index, node)
            {
                try
                {
                    if (x.dom(node).attr('custom-forms-data-required') || x.dom(node).attr('custom-forms-data-regexp'))
                    {
                        warning += x.ui.form.checkDataInput(node, options.tooltip);
                    }
                }
                catch (ex)
                {
                    x.debug.error(ex);
                }
            });
    
            if (warning === '')
            {
                return false;
            }
            else
            {
                alert(warning);
                return true;
            }
        },
        /*#endregion*/
    
        /*#region 函数:checkDataInput(node, warnTooltip)*/
        /*
        * 验证客户端数据
        */
        checkDataInput: function(node, warnTooltip)
        {
            // 如果没有id信息，或者为空则不检测
            if (typeof (node.id) === 'undefined' || node.id === '') { return ''; }
    
            var warning = '';
    
            if (warnTooltip == 1)
            {
                x.tooltip.newWarnTooltip({ element: node.id, hide: 1 });
            }
    
            if ($(node).hasClass('custom-forms-data-required'))
            {
                // 数据必填项验证
                if ($(node).val().trim() === '')
                {
                    var dataVerifyWarning = $(node).attr('dataVerifyWarning');
    
                    // x.debug.log('x:' + x.page.getElementLeft(node) + ' y:' + x.page.getElementTop(node));
    
                    if (dataVerifyWarning)
                    {
                        if (warnTooltip == 1)
                        {
                            x.tooltip.newWarnTooltip({ element: node.id, message: dataVerifyWarning, hide: 0 });
                        }
    
                        warning += dataVerifyWarning + '\n';
                    }
                }
            }
            // x-dom
            if ($(node).hasClass('x-dom-data-regexp'))
            {
                // 数据规则验证
                if ($(node).val().trim() !== '')
                {
                    if (!x.expressions.exists({ text: $(node).val(), ignoreCase: $(node).attr('dataIgnoreCase'), regexpName: $(node).attr('dataRegExpName'), regexp: $(node).attr('dataRegExp') }))
                    {
                        var dataRegExpWarning = $(node).attr('dataRegExpWarning');
    
                        // x.debug.log(x.page.getElementTop(node));
    
                        if (dataRegExpWarning)
                        {
                            if (warnTooltip == 1)
                            {
                                x.tooltip.newWarnTooltip({ element: node.id, message: dataRegExpWarning, hide: 0 });
                            }
    
                            warning += dataRegExpWarning + '\n';
                        }
                    }
                }
            }
    
            return warning;
        },
        /*#endregion*/
    
        /*#region 函数:serialize(options)*/
        /**
        * 序列化数据
        */
        serialize: function(options)
        {
            options = x.ext({}, x.dom.data.defaults, options || {});
    
            // 统一格式为大写
            options.storageType = options.storageType.toUpperCase();
    
            if (x.isUndefined(serializeHooks[options.storageType])) { x.debug.error('Not supported serialize[{"storageType":"' + options.storageType + '"}].'); }
    
            return serializeHooks[options.storageType](options);
        }
        /*#endregion*/
    };
    
    var serializeHooks = [];
    
    /*#region 函数:serializeHooks['NORMAL'](options)*/
    /**
    * 将表单数据序列化为普通格式数据
    * @private
    */
    serializeHooks['NORMAL'] = function(options)
    {
        var outString = '';
    
        var list = x.dom('*');
    
        x.each(list, function(index, node)
        {
            try
            {
                if (x.isUndefined(node.id) || node.id === '') { return; }
    
                var dataType = x.dom(node).attr(options.dataTypeAttributeName);
    
                if (!x.isUndefined(dataType) && dataType != null)
                {
                    switch (dataType)
                    {
                        case 'value':
                            outString += node.id + '=' + encodeURIComponent(x.dom(node).val().trim()) + '&';
                            break;
                        case 'html':
                            outString += node.id + '=' + encodeURIComponent(x.dom(node).html().trim()) + '&';
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (ex)
            {
                x.debug.error(ex);
            }
        });
    
        // 移除最后一个 & 符号
        outString = x.string.rtrim(outString, '&');
    
        return outString;
    };
    /*#endregion*/
    
    /*#region 函数:serializeHooks['JSON'](options)*/
    /**
    * 将表单数据序列化为JSON格式数据
    * @private
    */
    serializeHooks['JSON'] = function(options)
    {
        var outString = '';
    
        if (options.includeAjaxStorageNode)
        {
            outString = '{"ajaxStorage":{'
        }
    
        var list = x.dom('*');
    
        x.each(list, function(index, node)
        {
            try
            {
                if (x.isUndefined(node.id) || node.id === '') { return; }
    
                var dataType = x.dom(node).attr(options.dataTypeAttributeName);
    
                if (!x.isUndefined(dataType) && dataType != null)
                {
                    switch (dataType)
                    {
                        case 'value':
                            outString += '"' + node.id + '":"' + x.toSafeJSON(x.dom(node).val().trim()) + '",';
                            break;
                        case 'html':
                            outString += '"' + node.id + '":"' + x.toSafeJSON(x.dom(node).html().trim()) + '",';
                            break;
                        case 'checkbox':
                            outString += '"' + node.id + '":[';
    
                            if ($(document.getElementsByName(node.id)).size() === 0)
                            {
                                outString += '],';
                                break;
                            }
    
                            var checkboxGroupName = node.id;
    
                            $(document.getElementsByName(node.id)).each(function(index, node)
                            {
                                if (checkboxGroupName === node.name && node.type.toLowerCase() === 'checkbox')
                                {
                                    outString += '{"text":"' + $(node).attr('text') + '", "value":"' + x.toSafeJSON($(node).val()) + '", "checked":' + node.checked + '},';
                                }
                            });
    
                            if (outString.substr(outString.length - 1, 1) === ',')
                            {
                                outString = outString.substr(0, outString.length - 1);
                            }
    
                            outString += '],';
    
                            break;
    
                        case 'list':
                            outString += '"' + node.id + '":[';
    
                            if ($(this).find('.list-item').size() === 0)
                            {
                                outString += '],';
                                break;
                            }
    
                            $(this).find('.list-item').each(function(index, node)
                            {
                                outString += '[';
    
                                $(this).find('.list-item-colum').each(function(index, node)
                                {
                                    if ($(node).hasClass('data-type-html'))
                                    {
                                        outString += '"' + x.toSafeJSON($(node).html().trim()) + '",';
                                    }
                                    else
                                    {
                                        outString += '"' + x.toSafeJSON($(node).val().trim()) + '",';
                                    }
                                });
    
                                if (outString.substr(outString.length - 1, 1) === ',')
                                {
                                    outString = outString.substr(0, outString.length - 1);
                                }
    
                                outString += '],';
                            });
    
                            if (outString.substr(outString.length - 1, 1) === ',')
                            {
                                outString = outString.substr(0, outString.length - 1);
                            }
    
                            outString += '],';
    
                            break;
    
                        case 'table':
                            outString += '"' + node.id + '":[';
    
                            $('#' + node.id).find('tr').each(function(index, node)
                            {
                                if ($(this).find('.table-td-item').size() === 0)
                                {
                                    return;
                                }
    
                                outString += '[';
    
                                $(this).find('.table-td-item').each(function(index, node)
                                {
                                    if ($(node).hasClass('data-type-html'))
                                    {
                                        outString += '"' + x.toSafeJSON($(node).html().trim()) + '",';
                                    }
                                    else
                                    {
                                        outString += '"' + x.toSafeJSON($(node).val().trim()) + '",';
                                    }
                                });
    
                                if (outString.substr(outString.length - 1, 1) === ',')
                                    outString = outString.substr(0, outString.length - 1);
    
                                outString += '],';
                            });
    
                            if (outString.substr(outString.length - 1, 1) === ',')
                                outString = outString.substr(0, outString.length - 1);
    
                            outString += '],';
    
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (ex)
            {
                x.debug.error(ex);
            }
        });
    
        // 移除最后一个逗号
        outString = x.string.rtrim(outString, ',');
    
        if (options.includeAjaxStorageNode)
        {
            outString += '}}';
        }
    
        return outString;
    };
    /*#endregion*/
    
    /*#region 函数:serializeHooks['XML'](options)*/
    serializeHooks['XML'] = function(options)
    {
        var outString = '';
    
        if (typeof (options) == 'undefined')
        {
            options = { includeAjaxStorageNode: false };
        }
    
        if (options.includeAjaxStorageNode)
        {
            outString += '<?xml version="1.0" encoding="utf-8" ?>';
            outString += '<ajaxStorage>';
        }
    
        var list = x.dom('*');
    
        x.each(list, function(index, node)
        {
            try
            {
                if (x.isUndefined(node.id) || node.id === '') { return; }
    
                var dataType = x.dom(node).attr(options.dataTypeAttributeName);
    
                if (!x.isUndefined(dataType) && dataType != null)
                {
                    switch (dataType)
                    {
                        case 'value':
                            outString += '<' + node.id + '>' + x.cdata(x.dom(node).val().trim()) + '</' + node.id + '>';
                            break;
                        case 'html':
                            outString += '<' + node.id + '>' + x.cdata(x.dom(node).html().trim()) + '</' + node.id + '>';
                            break;
                        case 'select':
    
                            if ($(node).get(0).selectedIndex !== -1)
                            {
                                outString += '<' + node.id + '><![CDATA[' + x.toSafeJSON(x.dom(node).get(0)[$(node).get(0).selectedIndex].value.trim()) + ']]></' + node.id + '>';
                            }
                            else
                            {
                                outString += '<' + node.id + '></' + node.id + '>';
                            }
                            break;
    
                        case 'checkbox':
                            var checkboxs = document.getElementsByName("check" + node.id);
                            var checkboxsResult = "";
                            for (var i = 0; i < checkboxs.length; i++)
                            {
                                if (checkboxs[i].checked)
                                {
                                    checkboxsResult += checkboxs[i].value + ';';
                                }
                            }
    
                            if (checkboxsResult !== '')
                            {
                                checkboxsResult = checkboxsResult.substring(0, checkboxsResult.length - 1);
                                outString += '<' + node.id + '><![CDATA[' + checkboxsResult + ']]></' + node.id + '>';
                            }
                            else
                            {
                                var notSelectedDefaultValue = $(node).attr('notSelectedDefaultValue');
    
                                if (notSelectedDefaultValue == undefined)
                                {
                                    outString += '<' + node.id + '>' + x.cdata('') + '</' + node.id + '>';
                                }
                                else
                                {
                                    outString += '<' + node.id + '><![CDATA[' + notSelectedDefaultValue + ']]></' + node.id + '>';
                                }
                            }
                            break;
    
                        default:
                            break;
                    }
                }
            }
            catch (ex)
            {
                x.debug.error(ex);
            }
        });
    
        if (options.includeAjaxStorageNode)
        {
            outString += '</ajaxStorage>';
        }
    
        return outString;
    }
    /*#endregion*//*#region 函数:val()*/
    /**
     * 获取对象的值
     * @method val
     * @memberof x.dom
     */
    dom.fn.val = function(value)
    {
        var element = this[0];
    
        if (x.isUndefined(value))
        {
            return element.nodeType === 1 ? element.value : undefined;
        }
        else
        {
            x.each(this.results, function(index, node)
            {
                node.value = value;
            });
        }
    
        return this;
    };
    
    var valHooks = [];
    
    /*#endregion*/
    
    /*#region 函数:html()*/
    dom.fn.html = function(value)
    {
        if (x.isUndefined(value))
        {
            var element = this[0];
    
            return element.nodeType === 1 ? element.innerHTML : undefined;
        }
        else
        {
            x.each(this.results, function(index, node)
            {
                node.innerHTML = value;
            });
        }
    
        return this;
    };
    /*#endregion*/
    
    /*#region 函数:size()*/
    /**
     * 查看结果集记录数
     * @method size
     * @memberof x.dom
     */
    dom.fn.size = function()
    {
        return this.results.length;
    };
    /*#endregion*/
    
    /*#region 函数:css()*/
    /**
     * 设置样式
     * @method css
     * @memberof x.dom
     */
    dom.fn.css = function()
    {
        if (arguments.length == 1 && x.type(arguments[0]) == 'string')
        {
            var element = this[0];
    
            var args = Array.prototype.slice.call(arguments).slice(0);
    
            args.unshift(element);
    
            return x.css.style.apply(this, args);
        }
        else
        {
            var me = this;
    
            var originalArgs = Array.prototype.slice.call(arguments).slice(0);
    
            x.each(this.results, function(index, node)
            {
                var args = originalArgs.slice(0);
    
                args.unshift(node);
    
                x.css.style.apply(me, args);
            });
        }
    
        return this;
    };
    /*#endregion*/// -*- ecoding=utf-8 -*-
    
    /**
    * @namespace net
    * @memberof x
    * @description 网络
    */
    x.net = {
    
        /**
        * 默认配置信息
        */
        defaults: {
            // 异步请求的数据键值
            xhrDataKey: 'xhr-xml',
            // 获取客户端标识信息        
            getClientId: function () 
            {
                var element = x.query('#session-client-id');
    
                // 根据页面存放的 session-client-id 元素，获取客户端标识信息, 如果页面不存在 session-client-id 元素，则返回空值。
                return element == null ? '' : x.isUndefined(element.value, '');
            },
            // 获取客户端签名信息
            getClientSignature: function()
            {
                var element = x.query('#session-client-signature');
    
                // 根据页面存放的 session-client-signature 元素，获取签名信息, 如果页面不存在 session-client-signature 元素，则返回空值。
                return element == null ? '' : x.isUndefined(element.value, '');
            }, 
            // 获取时间信息
            getTimestamp: function () {
                var element = x.query('#session-timestamp');
    
                // 根据页面存放的 session-timestamp 元素，获取时间戳信息, 如果页面不存在 session-timestamp 元素，则返回空值。
                return element == null ? '' : x.isUndefined(element.value, '');
            }, 
            // 获取随机数信息
            getNonce: function () {
                var element = x.query('#session-nonce');
    
                // 根据页面存放的 session-nonce 元素，获取签名信息, 如果页面不存在 session-nonce 元素，则返回空值。
                return element == null ? '' : x.isUndefined(element.value, '');
            },
            // 获取等待窗口
            getWaitingWindow: function(options)
            {
                // 设置默认选项参数
                options = x.ext({
                    type: 'default',                        // 窗口类型
                    text: i18n.net.waiting.commitTipText    // 提示信息
                }, options || {});
    
                if (x.isUndefined(options.name))
                {
                    options.name = x.getFriendlyName(location.pathname + '$' + options.type + '$waiting$window');
                }
    
                var name = options.name;
    
                if (x.isUndefined(window[name]))
                {
                    if (options.type == 'mini')
                    {
                        window[name] = {
                            // 名称
                            name: name,
                            // 选项
                            options: options,
                            // 容器
                            container: null,
                            // 消息框
                            message: null,
    
                            /*#region 函数:create(text)*/
                            create: function(text)
                            {
                                if (document.getElementById(this.name + '-text') == null)
                                {
                                    $(document.body).append('<div id="' + this.name + '-container" class="x-ui-dialog-waiting-mini-window-container" ><div id="' + this.name + '-text" class="x-ui-dialog-waiting-mini-window-text" >' + text + '</div></div>');
                                }
                                else
                                {
                                    x.query('[id="' + this.name + '-text"]').innerHTML = text;
                                }
    
                                if (this.container === null)
                                {
                                    this.container = document.getElementById(this.name + '-container');
                                }
                            },
                            /*#endregion*/
    
                            /*#region 函数:show(text)*/
                            /*
                            * 显示
                            */
                            show: function()
                            {
                                if (!x.isUndefined(arguments[0]))
                                {
                                    this.options.text = arguments[0];
                                }
    
                                this.create(this.options.text);
    
                                // 设置弹出窗口的位置
                                x.css.style(this.container, {
                                    display: '',
                                    position: 'fixed',
                                    left: '4px',
                                    bottom: '4px'
                                });
                            },
                            /*#endregion*/
    
                            /*#region 函数:hide()*/
                            /*
                            * 隐藏
                            */
                            hide: function()
                            {
                                if (this.container != null)
                                {
                                    x.css.style(this.container, { display: 'none' });
    
                                    // this.container.style.display = 'none';
                                    // $(this.container).css({ display: '', opacity: this.maxOpacity });
                                    // (this.container).fadeOut((this.maxDuration * 2000), function()
                                    // {
                                    //    $(this.container).css({ display: 'none' });
                                    // });
                                }
                            }
                            /*#endregion*/
                        };
                    }
                    else
                    {
                        window[name] = {
                            // 实例名称
                            name: name,
    
                            // 配置信息
                            options: options,
    
                            // 遮罩
                            maskWrapper: null,
    
                            // 容器
                            container: null,
    
                            // 消息框
                            message: null,
    
                            // 等待窗口的锁
                            lock: 0,
    
                            // 延迟显示等待窗口
                            lazy: options.lazy ? options.lazy : 0,
    
                            maxOpacity: options.maxOpacity ? options.maxOpacity : 0.4,
    
                            maxDuration: options.maxDuration ? options.maxDuration : 0.2,
    
                            height: options.height ? options.height : 50,
    
                            width: options.width ? options.width : 200,
    
                            /*#region 函数:setPosition()*/
                            setPosition: function()
                            {
                                // 弹出窗口的位置
                                var range = x.page.getRange();
    
                                var pointX = (range.width - this.width) / 2;
                                var pointY = (range.height - this.height) / 3;
    
                                x.util.setLocation(this.container, pointX, pointY);
                            },
                            /*#endregion*/
    
                            /*#region 函数:createMaskWrapper()*/
                            createMaskWrapper: function()
                            {
                                var wrapper = document.getElementById(this.name + '$maskWrapper');
    
                                if (wrapper === null)
                                {
                                    $(document.body).append('<div id="' + this.name + '$maskWrapper" style="display:none;" ></div>');
    
                                    wrapper = document.getElementById(this.name + '$maskWrapper');
                                }
    
                                wrapper.className = 'x-ui-dialog-mask-wrapper';
    
                                wrapper.style.height = x.page.getRange().height + 'px';
                                wrapper.style.width = x.page.getRange().width + 'px';
    
                                if (wrapper.style.display === 'none')
                                {
                                    $(document.getElementById(this.name + '$maskWrapper')).css({ display: '', opacity: 0.1 });
                                    $(document.getElementById(this.name + '$maskWrapper')).fadeTo((this.maxDuration * 1000), this.maxOpacity, function()
                                    {
                                        // var mask = window[this.id];
    
                                        // $(document.getElementById(mask.popupWindowName)).css({ display: '' });
                                    });
                                }
                            },
                            /*#endregion*/
    
                            /*#region 函数:create(text)*/
                            create: function(text)
                            {
                                if (document.getElementById(this.name + '$text') == null)
                                {
                                    $(document.body).append('<div id="' + this.name + '$container" class="x-ui-dialog-waiting-window-container" ><div id="' + this.name + '$text" class="x-ui-dialog-waiting-window-text" >' + text + '</div></div>');
    
                                    this.createMaskWrapper();
                                }
                                else
                                {
                                    document.getElementById(this.name + '$text').innerHTML = text;
                                }
    
                                if (this.container === null)
                                {
                                    this.container = document.getElementById(this.name + '$container');
                                    this.maskWrapper = document.getElementById(this.name + '$maskWrapper');
                                }
                            },
                            /*#endregion*/
    
                            /*#region 函数:show(text)*/
                            /*
                            * 显示
                            */
                            show: function(text)
                            {
                                this.lock++;
    
                                var that = this;
    
                                var timer = x.newTimer(this.lazy, function(timer)
                                {
                                    if (that.lock > 0)
                                    {
                                        // x.debug.log('x.net.waitingWindow.lock:【' + that.lock + '】');
    
                                        if (that.maskWrapper === null)
                                        {
                                            that.maskWrapper = x.mask.newMaskWrapper(that.name + '$maskWrapper');
                                        }
    
                                        if (typeof (text) !== 'undefined')
                                        {
                                            that.options.text = text;
                                        }
    
                                        that.create(that.options.text);
    
                                        // 设置弹出窗口的位置
                                        var range = x.page.getRange();
    
                                        var pointX = (range.width - that.width) / 2;
                                        //var pointY = (range.height - this.height) / 3;
                                        var pointY = 120;
    
                                        x.util.setLocation(that.container, pointX, pointY);
    
                                        // 设置弹出窗口的位置
                                        that.container.style.display = '';
                                        that.maskWrapper.style.display = '';
                                    }
    
                                    timer.stop();
                                });
    
                                timer.start();
                            },
                            /*#endregion*/
    
                            /*#region 函数:hide()*/
                            /*
                            * 隐藏
                            */
                            hide: function()
                            {
                                this.lock--;
    
                                x.debug.log('x.net.waitingWindow.lock:【' + this.lock + '】');
    
                                if (this.lock === 0)
                                {
                                    if (this.container != null)
                                    {
                                        this.container.style.display = 'none';
                                    }
    
                                    if (this.maskWrapper != null && $(document.getElementById(this.name + '$maskWrapper')).css('display') !== 'none')
                                    {
                                        var that = this;
    
                                        $(document.getElementById(this.name + '$maskWrapper')).css({ display: '', opacity: this.maxOpacity });
                                        $(document.getElementById(this.name + '$maskWrapper')).fadeOut((this.maxDuration * 2000), function()
                                        {
                                            $(document.getElementById(that.name + '$maskWrapper')).css({ display: 'none' });
                                        });
                                    }
                                }
                            }
                            /*#endregion*/
                        };
                    }
                }
                else
                {
                    window[name].options = options;
                }
    
                return window[name];
            },
            /*#endregion*/
    
            // 捕获异常
            catchException: function(response, outputType)
            {
                try
                {
                    var result = x.toJSON(response);
    
                    if (!x.isUndefined(result) && !x.isUndefined(result.message) && !x.isUndefined(result.message.category) && result.message.category === 'exception')
                    {
                        if (outputType == 'console')
                        {
                            x.debug.error(result.message.description);
                        }
                        else
                        {
                            x.msg(result.message.description);
                        }
                    }
                }
                catch (ex)
                {
                    x.debug.error(ex);
                }
            }
        },
        /*#endregion*/
    
        /**
        * 发起网络请求
        * @method xhr
        * @memberof x.net
        * @param {object} [options] 选项<br /> 
        * 可选值范围: 
        * <table class="param-options" >
        * <thead>
        * <tr><th>名称</th><th>类型</th><th class="last" >描述</th></tr>
        * </thead>
        * <tbody>
        * <tr><td class="name" >type</td><td>string</td><td>HTTP请求类型(GET|POST)</td></tr>
        * <tr><td class="name" >url</td><td>string</td><td>地址</td></tr>
        * <tr><td class="name" >data</td><td>object</td><td>数据</td></tr>
        * <tr><td class="name" >async</td><td>boolean</td><td>是否是异步请求</td></tr>
        * </tbody>
        * </table>
        */
        xhr: function()
        {
            // -------------------------------------------------------
            // 可选择参数
            // waitingMessage   等待窗口显示的文本信息。
            // popResultValue   弹出回调结果。
            // callback         回调函数。
            // -------------------------------------------------------
    
            var url, xhrDataValue, options;
    
            if (arguments.length == 2 && x.type(arguments[1]) === 'object')
            {
                // 支持没有Xml数据，只有地址和回调函数的调用。
    
                url = arguments[0];
                options = arguments[1];
                xhrDataValue = '';
            }
            else if (arguments.length == 2 && x.type(arguments[1]) === 'string')
            {
                // 支持没有回调函数，只有地址和Xml数据的调用。
    
                url = arguments[0];
                options = {};
                xhrDataValue = arguments[1];
            }
            else if (arguments.length == 3 && x.type(arguments[1]) === 'string' && x.isFunction(arguments[2]))
            {
                // 支持没有回调函数，只有地址和Xml数据的调用。
    
                url = arguments[0];
                options = { callback: arguments[2] };
                xhrDataValue = arguments[1];
            }
            else
            {
                url = arguments[0];
                xhrDataValue = arguments[1];
                options = arguments[2];
            }
    
            options = x.ext(x.net.defaults, options);
    
            // 判断是否启用等待窗口
            var enableWaitingWindow = x.isFunction(options.getWaitingWindow)
                                        && !x.isUndefined(options.waitingMessage)
                                        && options.waitingMessage !== '';
    
            if (enableWaitingWindow)
            {
                // 开启等待窗口
                options.getWaitingWindow({ text: options.waitingMessage, type: x.isUndefined(options.waitingType, 'default') }).show();
            }
    
            var type = x.isUndefined(options.type, 'POST');
    
            var async = x.isUndefined(options.async, false);
    
            // 设置 data 值
            var data = x.ext({}, options.data || {});
    
            var xml = x.toXML(xhrDataValue, 1);
    
            if (xhrDataValue != '' && xml)
            {
                data[options.xhrDataKey] = xhrDataValue;
            }
            else if (!xml && xhrDataValue.indexOf('=') > 0)
            {
                // 非Xml字符格式, 普通的POST数据
                var list = xhrDataValue.split('&');
    
                x.each(list, function(index, node)
                {
                    var items = node.split('=');
    
                    if (items.length == 2)
                    {
                        data[items[0]] = decodeURIComponent(items[1]);
                    }
                });
            }
    
            if (x.isFunction(options.getClientId) && options.getClientId() != '')
            {
                data.clientId = options.getClientId();
    
                if (x.isFunction(options.getClientId) && options.getClientSignature() != '')
                {
                    data.clientSignature = options.getClientSignature();
                    data.timestamp = options.getTimestamp();
                    data.nonce = options.getNonce();
                }
            }
    
            // $.ajax 
            x.net.ajax({
                type: type,
                url: url,
                data: data,
                async: async,
                success: function(response)
                {
                    if (enableWaitingWindow)
                    {
                        // 关闭等待窗口
                        options.getWaitingWindow({ type: options.waitingType }).hide();
                    }
    
                    if (options.returnType == 'json')
                    {
                        // 捕获处理异常
                        options.catchException(response, options.outputException);
    
                        var result = x.toJSON(response).message;
    
                        switch (Number(result.returnCode))
                        {
                            case 0:
                                // 0:正确操作
                                if (!!options.popResultValue)
                                {
                                    x.msg(result.value);
                                }
    
                                x.call(options.callback, response);
                                break;
    
                            case -1:
                            case 1:
                                // -1:异常信息 | 1:错误信息
                                x.msg(result.value);
                                break;
                            default:
                                // 其他信息
                                x.msg(result.value);
                                break;
                        }
                    }
                    else
                    {
                        x.call(options.callback, response);
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown)
                {
                    x.debug.log(XMLHttpRequest.responseText);
    
                    if (x.isFunction(options.error))
                    {
                        options.error(XMLHttpRequest, textStatus, errorThrown);
                    }
                    else
                    {
                        if (XMLHttpRequest.status == 401)
                        {
                            x.msg(i18n.net.errors['401']);
                        }
                        else if (XMLHttpRequest.status == 404)
                        {
                            x.msg(i18n.net.errors['404']);
                        }
                        else if (XMLHttpRequest.status == 500)
                        {
                            x.msg(i18n.net.errors['500']);
                        }
                        else if (XMLHttpRequest.status != 0)
                        {
                            x.debug.error(i18n.net.errors['unkown'].format(XMLHttpRequest.status + (XMLHttpRequest.statusText != '' ? (' ' + XMLHttpRequest.statusText) : '')));
                        }
                    }
                }
            });
        },
        /*#endregion*/
    
        // 已加载的文件标识
        requireLoaded: {},
    
        /**
        * 通过Ajax方式加载样式和脚本
        */
        require: function(options)
        {
            options = x.ext({
                fileType: 'script',
                id: '',
                path: '',
                type: 'GET',
                async: true
            }, options || {});
    
            if (options.id != '' && x.net.requireLoaded[options.id])
            {
                x.debug.log('require file {"id":"{0}", path:"{1}"} exist. [ajax]'.format(options.id, options.path));
    
                x.call(options.callback);
    
                return true;
            }
    
            x.debug.log('require file {"id":"{0}", path:"{1}"} loading. [ajax]'.format(options.id, options.path));
    
            x.net.ajax(
            {
                type: options.type,
                url: options.path,
                async: options.async,
                success: function(responseText)
                {
                    x.debug.log('require file {"id":"{0}", path:"{1}"} finished. [ajax]'.format(options.id, options.path));
    
                    var head = document.getElementsByTagName("HEAD").item(0);
    
                    if (options.fileType == 'template')
                    {
                        var node = document.createElement("script");
                        node.type = "text/template";
                        node.src = options.path;
                    }
                    else if (options.fileType == 'css')
                    {
                        var node = document.createElement("style");
                        node.type = "text/css";
                        node.href = options.path;
                    }
                    else
                    {
                        var node = document.createElement("script");
    
                        node.language = "javascript";
                        node.type = "text/javascript";
                        node.src = options.path;
                    }
    
                    try
                    {
                        // IE8以及以下不支持这种方式，需要通过text属性来设置
                        node.appendChild(document.createTextNode(responseText));
                    }
                    catch (ex)
                    {
                        node.text = responseText;
                    }
    
                    if (options.id != '')
                    {
                        node.id = options.id;
                        x.net.requireLoaded[options.id] = true;
                    }
    
                    head.appendChild(node);
    
                    x.call(options.callback);
                }
            });
        },
    
        ajax: function(options)
        {
            var request = x.net.newHttpRequest(options);
    
            request.send();
        },
        /*#endregion*/
    
        newHttpRequest: function(options)
        {
            var request = {
                xhr: null,
                // 数据
                data: null,
                // 超时设置
                timeout: 90,
                // 是否已完成
                done: false,
    
                // 发送
                send: function()
                {
                    if (this.xhr == null)
                    {
                        this.xhr = x.net.newXmlHttpRequest();
    
                        if (!this.xhr)
                        {
                            x.debug.error('create xhr failed'); return false;
                        }
                    }
    
                    this.xhr.open(this.type, this.url, this.async);
    
                    var me = this;
    
                    x.event.add(this.xhr, "readystatechange", function()
                    {
                        var xhr = me.xhr;
    
                        // 监听状态
                        // x.debug.log('{0} readyState:{1} status:{2} done:{3}'.format(x.debug.timestamp(), xhr.readyState, xhr.status, me.done));
    
                        // 保持等待，直到数据完全加载，并保证请求未超时  
                        if (xhr.readyState == 4 && !me.done)
                        {
                            // 0 为访问的本地，200 到 300 代表访问服务器成功，304 代表没做修改访问的是缓存
                            if (xhr.status == 0 || (xhr.status >= 200 && xhr.status < 300) || xhr.status == 304)
                            {
                                // 成功则调用回调函数，并传入xhr对象  
                                x.call(me.success, xhr.responseText);
                            }
                            else
                            {
                                // 失败则调用error回调函数  
                                x.call(me.error, xhr, xhr.status);
                            }
    
                            // 避免内存泄漏，清理文档  
                            xhr = null;
                        }
                    });
    
                    // 如果请求超过 timeout 设置的时间没有响应, 则取消请求（如果尚未完成的话）  
                    setTimeout(function() { me.done = true; }, me.timeout * 1000);
    
                    if (this.type == 'POST')
                    {
                        this.xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                        this.xhr.send(x.serialize(this.data));
                    }
                    else
                    {
                        // 发送同步请求，如果浏览器为Chrome或Opera，必须发布后才能运行，不然会报错
                        this.xhr.send(null);
                    }
                },
    
                create: function(options)
                {
                    options = x.ext({
                        type: 'GET',
                        url: '',
                        data: {},
                        async: true,
                        timeout: 90
                    }, options || {});
    
                    this.type = options.type;
                    this.url = options.url;
                    this.data = options.data;
                    this.async = options.async;
                    this.timeout = options.timeout;
    
                    this.success = options.success;
                    this.error = options.error;
                }
            };
    
            // 初始化对象
            request.create(options);
    
            return request;
        },
    
        /*#region 函数:newXmlHttpRequest()*/
        /**
        * 创建 XMLHttpRequest 对象
        * @private
        */
        newXmlHttpRequest: function()
        {
            var xhr = null;
    
            if (window.ActiveXObject) // IE
            {
                try
                {
                    // IE6 以及以后版本中可以使用
                    xhr = new ActiveXObject("Msxml2.XMLHTTP");
                }
                catch (ex)
                {
                    //IE5.5 以及以后版本可以使用
                    xhr = new ActiveXObject("Microsoft.XMLHTTP");
                }
            }
            else if (window.XMLHttpRequest)
            {
                //Firefox，Opera 8.0+，Safari，Chrome
                xhr = new XMLHttpRequest();
            }
    
            return xhr;
        },
        /*#endregion*/
    
        /**
        * 请求信息
        * @namespace request
        * @memberof x.net
        */
        request: {
    
            /*#region 函数:find(key)*/
            /**
            * 获取请求地址中某个参数的值
            * @method find
            * @memberof x.net.request
            * @param {string} 参数的键
            * @returns {string} 参数的值
            */
            find: function(key)
            {
                var resultValue = '';
                var list = location.search.substr(1).split('&');
    
                for (var i = 0; i < list.length; i++)
                {
                    if (list[i].indexOf(key) === 0)
                    {
                        resultValue = decodeURIComponent(list[i].replace(key + '=', ''));
                        break;
                    }
                }
    
                return resultValue;
            },
            /*#endregion*/
    
            /*#region 函数:findAll()*/
            /**
            * 查找请求的全部信息, 返回的值是个JSON格式.
            * 获取请求地址中所有参数的值
            * @method findAll
            * @memberof x.net.request
            * @returns {object} JSON格式的对象
            */
            findAll: function()
            {
                var outString = '';
    
                var list = location.search.substr(1).split('&');
    
                var temp;
    
                outString = '{';
    
                if (list === '') { return; }
    
                for (var i = 0; i < list.length; i++)
                {
                    temp = list[i].split('=');
    
                    outString += '"' + temp[0] + '":"' + decodeURIComponent(temp[1]) + '"';
    
                    if (i < list.length - 1)
                        outString += ',';
                }
    
                outString += '}';
    
                return x.evalJSON(outString);
            },
            /*#endregion*/
    
            /*#region 函数:getRawUrl()*/
            /**
            * 获取附加了查询字符串的 URL 路径
            */
            getRawUrl: function()
            {
                return location.href.replace(location.origin, '');
            },
            /*#endregion*/
    
            /*#region 函数:hash(key)*/
            /*
            * 判断锚点
            */
            hash: function(key)
            {
                return location.hash === ('#' + key) ? true : false;
            }
            /*#endregion*/
        }
    };
    
    /*#region 私有函数:request_callback(response)*/
    /**
    * 网络请求的默认回调函数
    * @private
    */
    x.net.request_callback = function(response)
    {
        var result = x.toJSON(response).message;
    
        switch (Number(result.returnCode))
        {
            case 0:
                // 0:正确操作
                // alert(result.value);
                break;
            case -1:
            case 1:
                // -1:异常信息 | 1:错误信息
                x.msg(result.value);
                break;
            default:
                // 其他信息
                x.msg(result.value);
                break;
        }
    };
    /*#endregion*/

    
    /**
    * @namespace page
    * @memberof x
    * @description 页面管理
    */
    x.page = {
    
        /*#region 函数:back()*/
        /**
        * 返回上一个页面. window.history.back() 函数的别名
        * @method back
        * @memberof x.page
        */
        back: function()
        {
            window.history.back(arguments);
        },
        /*#endregion*/
    
        /*#region 函数:close()*/
        /**
        * 关闭窗口<br />
        * 注: 由于浏览器安全限制, 此方法只支持关闭以 _blank 方式打开的窗口.
        * @method close
        * @memberof x.page
        */
        close: function()
        {
            try
            {
                window.opener = null;
                window.open('', '_self');
                window.close();
            }
            catch (ex)
            {
                window.close();
            }
        },
        /*#endregion*/
    
        /*#region 函数:refreshParentWindow()*/
        /**
        * 刷新父级窗口
        * @method refreshParentWindow
        * @memberof x.page
        */
        refreshParentWindow: function()
        {
            if (typeof (window.opener) == 'object')
            {
                x.debug.error('未定义父级窗口。');
            }
    
            // 如果有父级窗口，调用父级窗口刷新函数
            if (x.type(window.opener) == 'object' && x.isFunction(window.opener.window$refresh$callback))
            {
                window.opener.window$refresh$callback();
            }
            else
            {
                x.debug.log('父级窗口未定义 window$refresh$callback() 函数。');
            }
        },
        /*#endregion*/
    
        /**
        * 获取页面范围信息
        * @method getRange
        * @memberof x.page
        */
        getRange: function()
        {
            var pageWidth, pageHeight, windowWidth, windowHeight;
    
            var xScroll, yScroll;
    
            if (window.innerHeight && window.scrollMaxY)
            {
                xScroll = window.innerWidth + window.scrollMaxX;
                yScroll = window.innerHeight + window.scrollMaxY;
            }
            else if (document.body.scrollHeight > document.body.offsetHeight)
            {
                // all but Explorer Mac
                xScroll = document.body.scrollWidth;
                yScroll = document.body.scrollHeight;
            }
            else
            {
                // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
                xScroll = document.body.offsetWidth;
                yScroll = document.body.offsetHeight;
            }
    
            //console.log('self.innerWidth:' + self.innerWidth);
            //console.log('document.documentElement.clientWidth:' + document.documentElement.clientWidth);
    
            if (window.innerHeight)
            {
                // all except Explorer
                if (document.documentElement.clientWidth)
                {
                    windowWidth = document.documentElement.clientWidth;
                }
                else
                {
                    windowWidth = window.innerWidth;
                }
    
                windowHeight = window.innerHeight;
            }
            else if (document.documentElement && document.documentElement.clientHeight)
            {
                // IE
                windowWidth = document.documentElement.clientWidth;
                windowHeight = document.documentElement.clientHeight;
            }
            else if (document.body)
            {
                // other Explorers
                windowWidth = document.body.clientWidth;
                windowHeight = document.body.clientHeight;
            }
    
            // for small pages with total height less then height of the viewport
            if (yScroll < windowHeight)
            {
                pageHeight = windowHeight;
            }
            else
            {
                pageHeight = yScroll;
            }
    
            //console.log("xScroll " + xScroll)
            //console.log("windowWidth " + windowWidth)
    
            // for small pages with total width less then width of the viewport
            if (xScroll < windowWidth)
            {
                pageWidth = xScroll;
            }
            else
            {
                pageWidth = windowWidth;
            }
    
            //console.log("pageWidth " + pageWidth)
    
            return {
                width: pageWidth,
                height: pageHeight,
                windowWidth: windowWidth,
                windowHeight: windowHeight
            };
        },
    
        /*
        * 获取页面高度
        */
        getHeight: function()
        {
            // return document.body.scrollHeight;
            return x.page.getRange().height;
        },
    
        /*
        * 获取页面宽度
        */
        getWidth: function()
        {
            // return document.body.scrollWidth;
            return x.page.getRange().width;
        },
    
        /*
        * 获取页面可视区域高度
        */
        getViewHeight: function()
        {
            // return document.documentElement.clientHeight;
            return x.page.getRange().windowHeight;
        },
    
        /*
        * 获取页面可视区域宽度
        */
        getViewWidth: function()
        {
            // return document.documentElement.clientWidth;
            return x.page.getRange().windowWidth;
        },
    
        /**
        * 获取元素在页面中的坐标 Top 坐标
        */
        getElementTop: function(element)
        {
            return element.getBoundingClientRect().top;
            /*
            var actualTop = element.offsetTop;
            var current = element.offsetParent;
            while (current !== null)
            {
            actualTop += current.offsetTop;
            current = current.offsetParent;
            }
            return actualTop;
            */
        },
    
        /**
        * 获取元素在页面中的坐标 Left 坐标
        */
        getElementLeft: function(element)
        {
            return element.getBoundingClientRect().left;
            /*
            var actualLeft = element.offsetLeft;
            var current = element.offsetParent;
            while (current !== null)
            {
            actualLeft += current.offsetLeft;
            current = current.offsetParent;
            }
            return actualLeft;
            */
        },
    
        /**
        * 获取元素在页面可视区域中的坐标 Top 坐标
        */
        getElementViewTop: function(element)
        {
            return element.getBoundingClientRect().top;
            /*
            var actualTop = element.offsetTop;
            var current = element.offsetParent;
            while (current !== null)
            {
            actualTop += current.offsetTop;
            current = current.offsetParent;
            }
            if (document.compatMode == "BackCompat")
            {
            var elementScrollTop = document.body.scrollTop;
            } else
            {
            var elementScrollTop = document.documentElement.scrollTop;
            }
            return actualTop - elementScrollTop;*/
        },
    
        /**
        * 获取元素在页面可视区域中的坐标 Left 坐标
        */
        getElementViewLeft: function(element)
        {
            return element.getBoundingClientRect().left;
            /* 
            var actualLeft = element.offsetLeft;
            var current = element.offsetParent;
            while (current !== null)
            {
            actualLeft += current.offsetLeft;
            current = current.offsetParent;
            }
            if (document.compatMode == "BackCompat")
            {
            var elementScrollLeft = document.body.scrollLeft;
            } else
            {
            var elementScrollLeft = document.documentElement.scrollLeft;
            }
            return actualLeft - elementScrollLeft;
            */
        },
    
        /**
        * 获取元素在页面可视区域中的坐标 Left 坐标
        */
        getElementAbsoluteTop: function(element)
        {
            var top = x.page.getElementViewTop(element);
    
            var parents = $(element).parents();
    
            for (var i = 0; i < parents.length; i++)
            {
                var parent = $(parents[i]);
    
                if (parent.css('position') === 'absolute' && parent.css('top') !== 'auto')
                {
                    top = top - Number(parent.css('top').replace('px', ''));
                }
    
                x.page.getElementViewTop(parents[i])
            }
    
            return top;
        },
    
        /**
        * 获取元素的范围
        */
        getElementRange: function(element)
        {
            var display = $(element).css('display');
    
            // Safari bug
            if (display != 'none' && display != null)
            {
                return { width: element.offsetWidth, height: element.offsetHeight };
            }
    
            // All *Width and *Height properties give 0 on elements with display none,
            // so enable the element temporarily
            var style = element.style;
    
            var originalVisibility = style.visibility;
            var originalPosition = style.position;
            var originalDisplay = style.display;
    
            style.visibility = 'hidden';
            style.position = 'absolute';
            style.display = 'block';
    
            var originalWidth = element.clientWidth;
            var originalHeight = element.clientHeight;
    
            style.display = originalDisplay;
            style.position = originalPosition;
            style.visibility = originalVisibility;
    
            return { width: originalWidth, height: originalHeight };
        },
    
        /**
        * 返回页面顶部
        */
        goTop: function()
        {
            window.scrollTo(0, 0);
            // document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        },
    
        /*
        * 竖向滚动
        */
        scroll: function(value)
        {
            window.scrollTo(0, value);
        },
    
        /**
        * 重新调整元素的大小
        *
        * @element	: 元素
        * @height	: 高度
        * @width	: 宽度
        */
        resize: function(element, height, width)
        {
            $(element).css({
                height: document.body.clientHeight - height + "px",
                width: document.body.clientWidth - width + "px"
            });
        },
    
        /*
        * 模拟窗口最大化.
        */
        setFullScreen: function()
        {
            // 作废
            //
            // var width = screen.availWidth - (document.layers ? 10 : -8);
            // var height = screen.availHeight - (document.layers ? 20 : -8);
    
            // window.resizeTo(width, height);
    
            // x.Page.setTitle(width + '-' + height);
        },
    
        /**
        * 打印Xml的文档
        *
        * @text : 文本信息
        */
        printXml: function(text)
        {
            if (text == null) { return ''; }
    
            return text.replace(/</g, '&lt;').replace(/>/g, '&gt;');
        },
    
        /**
        * 禁止拷贝
        */
        forbidCopy: {
            /*
            * Hotkey 即为热键的键值,是ASII码.
            * Ctronl : 17.
            * C : 99
            * V :
            */
            hotkeys: [67],
    
            message: '当前页面【禁止拷贝】信息。',
    
            /*#region 函数:listen()*/
    
            listen: function()
            {
                if ($(document.getElementById('forbidCopy$activate')).val() === '1')
                {
                    // 禁止拷贝
                    x.page.forbidCopy.activate();
                }
            },
            /*#endregion*/
    
            /*#region 函数:activate(e)*/
            activate: function(e)
            {
                var event = window.event ? window.event : e;
    
                if (document.layers)
                {
                    document.captureEvents(event.MOUSEDOWN);
    
                    document.captureEvents(event.KEYDOWN);
                }
    
                // 上下文菜单
                document.oncontextmenu = function() { return false; };
    
                // 全选功能
                document.onselectstart = function(event)
                {
                    event = x.getEventObject(event);
    
                    event.returnValue = false;
                };
    
                // 鼠标
                document.onmousedown = x.page.forbidCopy.mouse;
    
                // 键盘
                document.onkeydown = x.page.forbidCopy.keyboard;
            },
            /*#endregion*/
    
            /*#region 函数:mouse(e)*/
            mouse: function(e)
            {
                var event = window.event ? window.event : e;
    
                if (document.all)
                {
                    if (event.button == 1 || event.button == 2 || event.button == 3)
                    {
                        window.document.oncontextmenu = function() { return false; }
                    }
                }
    
                if (document.layers)
                {
                    if (event.which == 3)
                    {
                        window.document.oncontextmenu = function() { return false; }
                    }
                }
            },
            /*#endregion*/
    
            /*#region 函数:keyboard(e)*/
            keyboard: function(e)
            {
                var event = window.event ? window.event : e;
    
                var hotkeys = x.page.forbidCopy.hotkeys;
    
                var result = false;
    
                for (var i = 0; i < hotkeys.length; i++)
                {
                    if (hotkeys[i] == event.keyCode)
                    {
                        result = true;
                        break;
                    }
                }
    
                // event.shiftKey | event.altKey | event.ctrlKey
                if (event.ctrlKey || result)
                {
                    alert(x.page.forbidCopy.message);
    
                    x.debug.log(x.page.forbidCopy.message);
    
                    return false;
                }
            }
            /*#endregion*/
        },
    
        /*
        * 创建分页对象
        */
        newPagingHelper: function(pageSize)
        {
            if (pageSize === undefined || pageSize === '') { pageSize = 10; }
    
            var helper = {
    
                rowIndex: 0,
    
                pageSize: pageSize,
    
                rowCount: 0,
    
                pageCount: 0,
    
                currentPage: 0,
    
                firstPage: 0,
    
                previousPage: 0,
    
                nextPage: 0,
    
                lastPage: 0,
    
                query: {
                    table: '', fields: '', where: {}, orders: ''
                },
    
                /*
                * 加载对象信息
                */
                load: function(paging)
                {
                    if (!x.isUndefined(paging.pageSize)) { this.pagingize = Number(paging.pageSize); }
    
                    if (!x.isUndefined(paging.rowCount)) { this.rowCount = Number(paging.rowCount); }
    
                    if (!x.isUndefined(paging.rowIndex)) { this.rowIndex = Number(paging.rowIndex); }
    
                    if (!x.isUndefined(paging.firstPage)) { this.firstPage = Number(paging.firstPage); }
    
                    if (!x.isUndefined(paging.previousPage)) { this.previousPage = Number(paging.previousPage); }
    
                    if (!x.isUndefined(paging.nextPage)) { this.nextPage = Number(paging.nextPage); }
    
                    if (!x.isUndefined(paging.lastPage)) { this.lastPage = Number(paging.lastPage); }
                },
    
                /*
                * 设置上一页的参数.
                */
                setPreviousPage: function(value)
                {
                    this.previousPage = value - 1;
    
                    if (this.previousPage < 1)
                    {
                        this.previousPage = 1;
                    }
                },
    
                /**
                * 设置下一页的参数.
                */
                setNextPage: function(value)
                {
                    this.nextPage = value + 1;
    
                    if (this.nextPage > this.lastPage)
                    {
                        this.nextPage = this.lastPage;
                    }
                },
    
                /**
                * 设置页数
                */
                getPagesNumber: function(format, value, length)
                {
                    // may be overwrite here. ^_^
                    //
                    // x.page.newPagesHelper.prototype.getPagesNumber
                    //
    
                    var outString = '';
    
                    var page = value;
    
                    var counter;
    
                    if (value - length > 0)
                    {
                        value -= length;
                    }
                    else
                    {
                        value = 1;
                    }
    
                    counter = value + (length * 2) + 1;
    
                    if (counter > this.lastPage)
                    {
                        value = this.lastPage - (length * 2);
                    }
    
                    for (var i = value; i < counter; i++)
                    {
                        if (i < 1) { continue; }
    
                        if (i > this.lastPage) { break; }
    
                        if (format.indexOf('{0}') > -1)
                        {
                            outString += '<a href="' + format.replace('{0}', i) + '" >';
                        }
                        else
                        {
                            outString += '<a href="javascript:' + format + '(' + i + ');" >';
                        }
    
                        outString += ((page == i) ? ('<strong>' + i + '</strong>') : i);
                        outString += '</a> ';
                    }
    
                    return outString;
                },
    
                /**
                * 解析为分页菜单信息
                */
                tryParseMenu: function(format)
                {
                    var outString = '';
    
                    outString += '<div class="nav-pager" >';
                    outString += '<div class="nav-pager-1" >';
                    outString += '共有' + this.rowCount + '条信息 当前' + (this.rowIndex + 1) + '-' + (this.rowIndex + this.pageSize) + '信息 ';
    
                    if (format.indexOf('{0}') > -1)
                    {
                        outString += '<a href="' + format.replace('{0}', this.firstPage) + '">首页</a> ';
                        outString += '<a href="' + format.replace('{0}', this.previousPage) + '">上一页</a> ';
                        outString += this.getPagesNumber(format, this.currentPage, 2)
                        outString += '<a href="' + format.replace('{0}', this.nextPage) + '">下一页</a> ';
                        outString += '<a href="' + format.replace('{0}', this.lastPage) + '">末页</a> ';
                    }
                    else
                    {
                        outString += '<a href="javascript:' + format + '(' + this.firstPage + ');">首页</a> ';
                        outString += '<a href="javascript:' + format + '(' + this.previousPage + ');">上一页</a> ';
                        outString += this.getPagesNumber(format, this.currentPage, 2)
                        outString += '<a href="javascript:' + format + '(' + this.nextPage + ');">下一页</a> ';
                        outString += '<a href="javascript:' + format + '(' + this.lastPage + ');">末页</a> ';
                    }
    
                    outString += '</div>';
                    outString += '<div class="clear" ></div>';
                    outString += '</div>';
    
                    return outString;
                },
    
                // 序列化查询信息
                toQueryXml: function()
                {
                    var outString = '';
    
                    outString += '<query>';
    
                    if (this.query.table.length > 0) outString += '<table><![CDATA[' + this.query.table + ']]></table>';
                    if (this.query.fields.length > 0) outString += '<fields><![CDATA[' + this.query.fields + ']]></fields>';
    
                    var where = '<where>';
                    x.each(this.query.where, function(name, value)
                    {
                        where += '<key name="' + name + '" ><![CDATA[' + value + ']]></key>';
                    });
                    where += '</where>';
                    if (where != '<where></where>') outString += where;
    
                    if (this.query.orders.length > 0) outString += '<orders><![CDATA[' + this.query.orders + ']]></orders>';
    
                    outString += '</query>';
    
                    if (outString == '<query></query>') outString += '';
                    
                    return outString;
                },
    
                toXml: function()
                {
                    var outString = '';
    
                    outString += '<paging>';
                    outString += '<rowIndex>' + this.rowIndex + '</rowIndex>';
                    outString += '<pageSize>' + this.pageSize + '</pageSize>';
                    outString += '<rowCount>' + this.rowCount + '</rowCount>';
                    outString += '<pageCount>' + this.pageCount + '</pageCount>';
                    outString += '<currentPage>' + this.currentPage + '</currentPage>';
                    outString += '<firstPage>' + this.firstPage + '</firstPage>';
                    outString += '<previousPage>' + this.previousPage + '</previousPage>';
                    outString += '<nextPage>' + this.nextPage + '</nextPage>';
                    outString += '<lastPage>' + this.lastPage + '</lastPage>';
                    outString += '<lastPage>' + this.lastPage + '</lastPage>';
                    outString += '</paging>';
                    outString += this.toQueryXml();
                    
                    return outString;
                }
            };
    
            return helper;
        }
    }
    
    // 输出对象
        
    if (typeof define === "function" && define.amd ) {
	    // RequireJS && SeaJS
        define(function() { return x; });
    } else if (typeof module !== "undefined" && module.exports ) {
	    // Common-JS && NodeJS
        module.exports = x;
    } else {
        // Browser
        window.x = x;
    }

    return x;
}));