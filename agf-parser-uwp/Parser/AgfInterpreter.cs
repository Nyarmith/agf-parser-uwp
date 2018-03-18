//=========================================================
//====== Adventure-Game-Format Language Interpreter =======
//====== Author: Sergey Ivanov .  License: MIT      =======
//=========================================================

using System;
using System.Collections.Generic;

namespace AgfLang
{
    public enum Tokens { SEMI, EQ, PEQ, SEQ, MEQ, DEQ, OR, AND, GT, GEQ, LT, LEQ,
                         ISEQ, NEQ, PLUS, MINUS, MUL, DIV, LPAREN, RPAREN, ID, INT, EOF };

    class Token
    {
        public Tokens type;
        public string value;
        public Token() { }
        public Token(Tokens t_, string v_) { type = t_; value = v_; }
        public override string ToString()  { return String.Format("Token({0},{1})", type, value); }
    }

    //visitor pattern
    interface Visitable   { string accept(NodeVisitor visitor); }
    interface NodeVisitor
    {
        string visit(AST node);
        string visit(BinOp node);
        string visit(UnOp node);
        string visit(Assign node);
        string visit(Var node);
        string visit(Int node);
        string visit(Compound node);
    }

    //========================
    //==== AST Node Types ====
    //========================
    class AST : Visitable
    {
        public Token token;
        public AST() { }
        public AST(Token token_) { token = token_; }
        public virtual string accept(NodeVisitor visitor) { return visitor.visit(this); }
    }

    class BinOp : AST
    {
        public AST left, right;
        public BinOp(AST left_, Token op_, AST right_)
        {
            left  = left_;
            token = op_;
            right = right_;
        }
        public override string accept(NodeVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    class UnOp : AST
    {
        public AST value;
        public UnOp(Token op_, AST value_)
        {
            token = op_;
            value = value_;
        }
        public override string accept(NodeVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    class Var : AST
    {
        public Var(Token token_) : base(token_) { }
        public override string accept(NodeVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    class Int : AST
    {
        public Int(Token token_) : base(token_) { }
        public override string accept(NodeVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    class Assign : AST
    {
        public Var left;
        public AST right;
        public Assign(Var left_, Token op_, AST right_)
        {
            left  = left_;
            token = op_;
            right = right_;
        }
        public override string accept(NodeVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    class Compound : AST
    {
        public List<AST> children = new List<AST>();
        public override string accept(NodeVisitor visitor)
        {
            return visitor.visit(this);
        }
    }

    //========================
    //====Lexer Definition====
    //========================
    class Lexer
    {
        string text;
        int position;
        char current_char;
        public Lexer(string text_)
        {
            text = text_;
            position = 0;
            current_char = text[position];
        }

        void error()  //Signal error during scanning
        {
            throw new Exception("Error During Scanning: Invalid Character Line " + Convert.ToString(position));
        }

        void advance()
        {
            position++;
            if (position >= text.Length)
                current_char = '\0';
            else
                current_char = text[position];
        }

        void skipWhiteSpace()
        {
            while (current_char != '\0' && Char.IsWhiteSpace(current_char))
                advance();
        }

        char peek()
        {
            int peekpos = position + 1;
            if (peekpos >= text.Length)
                return '\0';
            return text[peekpos];
        }

        //get var::name at current position
        Token id()
        {
            string fst = "";
            while(current_char != ':' && Char.IsLetterOrDigit(current_char) && current_char != '\0')
            {
                fst += current_char;
                advance();
            }
            //we've reached the first ':'
            if (current_char != ':' || peek() != ':')
                error();
            advance();
            advance();
            string snd = "";
            while(current_char != '\0' && Char.IsLetterOrDigit(current_char))
            {
                snd += current_char;
                advance();
            }
            return new Token(Tokens.ID, fst + ":" + snd); //make it easy to Split() later
        }

        //scan current numeric string
        string integer()
        {
            string ret = "";

            while (current_char != '\0' && Char.IsDigit(current_char))
            {
                ret += current_char;
                advance();
            }
            return ret;
        }

        //the actual lexing
        public Token getNextToken()
        {
            while(current_char != '\0')
            {
                if (Char.IsWhiteSpace(current_char))
                {
                    skipWhiteSpace();
                    continue;
                }

                //ID and INT tokens
                if (Char.IsDigit(current_char))
                    return new Token(Tokens.INT, integer());
                if (Char.IsLetterOrDigit(current_char))
                    return id();

                // Assignment Tokens
                if (current_char == '=' && peek() != '=')
                {
                    advance();
                    return new Token(Tokens.EQ, "=");
                }
                if (current_char == '*' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.MEQ, "*=");
                }
                if (current_char == '/' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.DEQ, "/=");
                }
                if (current_char == '+' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.PEQ, "+=");
                }
                if (current_char == '-' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.SEQ, "-=");
                }

                //Arithmetic Tokens
                if(current_char == '+')
                {
                    advance();
                    return new Token(Tokens.PLUS,"+");
                }
                if(current_char == '-')
                {
                    advance();
                    return new Token(Tokens.MINUS,"-");
                }
                if(current_char == '*')
                {
                    advance();
                    return new Token(Tokens.MUL,"*");
                }
                if(current_char == '/')
                {
                    advance();
                    return new Token(Tokens.DIV,"/");
                }

                // Logical and Grouping Tokens
                if (current_char == '|' && peek() == '|')
                {
                    advance();
                    advance();
                    return new Token(Tokens.OR,"||");
                }
                if (current_char == '&' && peek() == '&')
                {
                    advance();
                    advance();
                    return new Token(Tokens.AND, "&&");
                }
                if (current_char == ')')
                {
                    advance();
                    return new Token(Tokens.RPAREN, ")");
                }
                if (current_char == '(')
                {
                    advance();
                    return new Token(Tokens.LPAREN,"(");
                }

                // Comparison Tokens
                if (current_char == '=' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.ISEQ, "==");
                }
                if (current_char == '!' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.NEQ, "!=");
                }
                if (current_char == '>' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.GEQ, ">=");
                }
                if (current_char == '<' && peek() == '=')
                {
                    advance();
                    advance();
                    return new Token(Tokens.LEQ, "<=");
                }
                if (current_char == '>')
                {
                    advance();
                    return new Token(Tokens.GT, ">");
                }
                if (current_char == '<')
                {
                    advance();
                    return new Token(Tokens.LT, "<");
                }
                if (current_char == ';')
                {
                    advance();
                    return new Token(Tokens.SEMI, ";");
                }
                error();
            }
            return new Token(Tokens.EOF, "\0");
        }
    }

    //=========================
    //====Parser Definition====
    //=========================
    /*
     * statement_list  =  statement | statement SEMI statment_list | satement SEMI EOF
     * statement       =  assign | orexpr
     * assign          =  ID (EQ|PEQ|SEQ|MEQ|DEQ) orexpr
     * orexpr          =  andexpr ((OR) andexpr)*
     * andexpr         =  compexpr ((AND) compexpr)*
     * compexpr        =  sumexpr (ISEQ|GT|GEQ|LT|LEQ) sumexpr | sumexpr
     * sumexpr         =  mulexpr ((PLUS|MINUS) mulexpr)*
     * mulexpr         =  factor ((MUL|DIV) factor)*
     * factor          =  LPAREN orexpr RPAREN | ID | INT | NEG factor
     */

    class Parser
    {
        Lexer lexer;
        Token current_token;
        public Parser(Lexer lexer_)
        {
            lexer = lexer_;
            current_token = lexer.getNextToken();
        }

        private void error()
        {
            throw new Exception("Invalid Syntax; " + current_token);
        }

        private void eat(Tokens token_type)
        {
            if (current_token.type == token_type)
                current_token = lexer.getNextToken();
            else
                error();
        }

        public AST exec()
        {
            return statement_list();
        }

        public AST eval()
        {
            return orexpr();
        }

        //statement_list : statement | statement SEMI statement_list
        private AST statement_list()
        {
            Compound stmt_list = new Compound();
            stmt_list.children.Add(statement());
            while (current_token.type == Tokens.SEMI)
            {
                eat(Tokens.SEMI);
                if (current_token.type != Tokens.EOF)
                    stmt_list.children.Add(statement());
                else
                    break;
            }
            return stmt_list;
        }

        //statement : assign | orexpr
        private AST statement()
        {
            Token tkn = current_token;

            Tokens[] assignment_operators = new Tokens[] { Tokens.EQ, Tokens.SEQ, Tokens.PEQ, Tokens.MEQ, Tokens.DEQ };

            //taking care of assignment here
            //assign : ID (EQ|PEQ|SEQ|MEQ|DEQ) orexpr
            if (current_token.type == Tokens.ID)
            {
                eat(Tokens.ID);
                if (Array.IndexOf(assignment_operators, current_token.type) != -1)
                {
                    Token assign = current_token;
                    eat(assign.type);
                    return new Assign(new Var(tkn),assign, orexpr());
                }
            }
            //current token not an ID so it must be an orexpr
            return orexpr();
        }

        //orexpr : andexpr ((OR) andexpr)*
        private AST orexpr()
        {
            AST node = andexpr();
            while(current_token.type == Tokens.OR)
            {
                Token t = current_token;
                eat(Tokens.OR);
                node = new BinOp(node, t, andexpr());
            }
            return node;
        }

        //orexpr : compexpr ((AND) compexpr)*
        private AST andexpr()
        {
            AST node = compexpr();
            while(current_token.type == Tokens.AND)
            {
                Token t = current_token;
                eat(Tokens.AND);
                node = new BinOp(node, t, compexpr());
            }
            return node;
        }

        //compexpr : sumexpr (ISEQ|GT|GEQ|LT|LEQ) sumexpr | sumexpr
        private AST compexpr()
        {
            AST left = sumexpr();
            Tokens[] comparison_operators = new Tokens[] { Tokens.ISEQ, Tokens.GT, Tokens.GEQ, Tokens.LT, Tokens.LEQ, Tokens.NEQ };
            if (Array.IndexOf(comparison_operators, current_token.type) != -1)
            {
                Token t = current_token;
                eat(current_token.type);
                return new BinOp(left, t, sumexpr());
            }
            else
            {
                return left;
            }
        }

        //sumexpr : mulexpr ((PLUS|MINUS) mulexpr)*
        private AST sumexpr()
        {
            AST node = mulexpr();
            Tokens[] addition_operators = new Tokens[] { Tokens.PLUS, Tokens.MINUS };
            while (Array.IndexOf(addition_operators, current_token.type) != -1)
            {
                Token t = current_token;
                eat(current_token.type);
                node = new BinOp(node, t, mulexpr());
            }
            return node;
        }

        //mulexpr : factor ((MUL|DIV) factor)*
        private AST mulexpr()
        {
            AST node = factor();
            Tokens[] addition_operators = new Tokens[] { Tokens.MUL, Tokens.DIV };
            while (Array.IndexOf(addition_operators, current_token.type) != -1)
            {
                Token t = current_token;
                eat(current_token.type);
                node = new BinOp(node, t, factor());
            }
            return node;
        }

        //factor : INT | ID | LPAREN orexpr RPAREN | NEG factor
        private AST factor()
        {
            Token tkn = current_token;
            if (current_token.type == Tokens.INT)
            {
                eat(Tokens.INT);
                return new Int(tkn);
            }
            if (current_token.type == Tokens.ID)
            {
                eat(Tokens.ID);
                return new Var(tkn);
            }
            if (current_token.type == Tokens.LPAREN)
            {
                eat(Tokens.LPAREN);
                AST node = orexpr();
                eat(Tokens.RPAREN);
                return node;
            }
            if (current_token.type == Tokens.MINUS)
            {
                eat(Tokens.MINUS);
                AST node = factor();
                return new UnOp(tkn, node);
            }
            error();
            return new AST();
        }
    }

    //==============================
    //====Interpreter Definition====
    //==============================
    class Interpreter : NodeVisitor
    {
        private Dictionary<String, Dictionary<String, int>> states;
        private Lexer lexer;
        private Parser parser;

        public Interpreter(ref Dictionary<String, Dictionary<String,int>> usermem)
        {
            states = usermem;
        }

        //evaluate input to modify states
        public void exec(string input)
        {
            lexer  = new Lexer(input);
            parser = new Parser(lexer);

            AST root = parser.exec();
            root.accept(this);
        }

        //evaluate single expression and return value
        public int eval(string input)
        {
            lexer  = new Lexer(input);
            parser = new Parser(lexer);

            AST root = parser.eval();
            return Convert.ToInt32(root.accept(this));
        }

        public string visit(AST node)
        {
            throw new Exception("Error, Unhandled Node Type");
        }

        private int bool2int(bool t) { return ((t == true) ? 1 : 0); }

        public string visit(BinOp node)
        {

            int left   = Convert.ToInt32(node.left.accept(this));
            int right  = Convert.ToInt32(node.right.accept(this));
            int result = 0;
            
            switch (node.token.type)
            {
                case Tokens.PLUS:
                    result = left + right;
                    break;
                case Tokens.MINUS:
                    result = left - right;
                    break;
                case Tokens.MUL:
                    result = left * right;
                    break;
                case Tokens.DIV:
                    result = left / right;
                    break;
                case Tokens.LT:
                    result = bool2int(left < right);
                    break;
                case Tokens.GT:
                    result = bool2int(left > right);
                    break;
                case Tokens.GEQ:
                    result = bool2int(left >= right);
                    break;
                case Tokens.LEQ:
                    result = bool2int(left <= right);
                    break;
                case Tokens.ISEQ:
                    result = bool2int(left == right);
                    break;
                case Tokens.NEQ:
                    result = bool2int(left != right);
                    break;
                case Tokens.AND:
                    result = bool2int(left != 0 && right != 0);
                    break;
                case Tokens.OR:
                    result = bool2int(left != 0 || right != 0);
                    break;
            }
            return Convert.ToString(result);
        }

        public string visit(UnOp node)
        {
            int ret = Convert.ToInt32(node.value.accept(this));
            switch (node.token.type)
            {
                case Tokens.MINUS:
                    ret = -ret;
                    break;
            }
            return Convert.ToString(ret);
        }

        private int getvar(string varname)
        {
            //split variable node into first and second parts, then verify 
            string[] name = varname.Split(':');  //should be a name token, that we've tokenized to split conveniently

            if (!states.ContainsKey(name[0]))
                states[name[0]] = new Dictionary<string, int>();
            if (!states[name[0]].ContainsKey(name[1]))
                states[name[0]][name[1]] = 0;
            return states[name[0]][name[1]];
        }

        private void setvar(string varname, int val)
        {
            string[] name = varname.Split(':');  //should be a name token, that we've tokenized to split conveniently

            if (!states.ContainsKey(name[0]))
                states[name[0]] = new Dictionary<string, int>();
            states[name[0]][name[1]] = val;
        }

        public string visit(Assign node)
        {
            string name = node.left.token.value;
            int varval = getvar(name);  //Also initializes element if not there
            int right = Convert.ToInt32(node.right.accept(this));
            switch (node.token.type) //which operator are you?
            {
                case (Tokens.EQ):
                    setvar(name, right);
                    break;
                case (Tokens.PEQ):
                    setvar(name,  varval + right);
                    break;
                case (Tokens.SEQ):
                    setvar(name, varval - right);
                    break;
                case (Tokens.MEQ):
                    setvar(name, varval * right);
                    break;
                case (Tokens.DEQ):
                    setvar(name, varval / right);
                    break;
            }
            return Convert.ToString(getvar(name));
        }

        public string visit(Int node)
        {
            return node.token.value;
        }

        public string visit(Var node)
        {
            return Convert.ToString(getvar(node.token.value));
        }

        public string visit(Compound node)
        {
            string ret = "";
            foreach (AST n in node.children)
            {
                ret = n.accept(this);
            }
            return ret;  //just return the last one I guess
        }
    }

    public class AgfInterpreter
    {
        private Interpreter interpreter;
        private Dictionary<string, Dictionary<string, int>> mem_copy;

        public AgfInterpreter(ref Dictionary<string, Dictionary<string, int>> internal_mem)
        {
            mem_copy = internal_mem;
            interpreter = new Interpreter(ref internal_mem);
        }

        public string eval(string text)
        {
            int ret = interpreter.eval(text);
            return Convert.ToString(ret);
        }

        public void exec(string text)
        {
            interpreter.exec(text);
        }

        public string ShowMemory()
        {
            string ret = "";
            foreach (string k1 in mem_copy.Keys)
            {
                foreach (string k2 in mem_copy[k1].Keys)
                {
                    ret += k1 + "::" + k2 + "  :=  " + Convert.ToString(mem_copy[k1][k2]);
                    ret += '\n';
                }
            }
            return ret;
        }
    }
}
