using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARX
{

    public abstract class ARX_Rule<T>
    {
        /// <summary>
        /// Conditional function that decides if this rule is to apply to an input.
        /// If true, the positive action will run.
        /// If false, the negative action will run.
        /// </summary>
        /// <param name="oInput"></param>
        /// <returns></returns>
        public abstract bool Condition(T oInput);

        /// <summary>
        /// Action that will be taken if this rule's condition applies to the input.
        /// Return false to consume the input.
        /// </summary>
        /// <returns></returns>
        public virtual bool PositiveAction() { return true; }

        /// <summary>
        /// Action that will be taken if this rule's condition DOES NOT apply to the input.
        /// Return false to consume the input.
        /// </summary>
        /// <returns></returns>
        public virtual bool NegativeAction() { return true; }
    }

    /// <summary>
    /// Data structure that receives an input and applies a list of rules
    /// </summary>
    /// <typeparam name="T_MessageInput"></typeparam>
    public abstract class ARX_RuleSet<T_MessageInput>
    {
        /// <summary>
        /// Function ran when the input is passed out of this ruleset
        /// without being consumed.
        /// </summary>
        /// <param name="oInput"></param>
        public abstract void PassInputOn(T_MessageInput oInput);

        /// <summary>
        /// Function ran when a rule requests a ruleset to consume an input, stopping
        /// it from being processed further.
        /// </summary>
        /// <param name="oInput"></param>
        public abstract void ConsumeInput(T_MessageInput oInput);

        /// <summary>
        /// The list of rules in this ruleset.
        /// </summary>
        public List<ARX_Rule<T_MessageInput>> moa_rules = new List<ARX_Rule<T_MessageInput>>();

        public void AddRule(ARX_Rule<T_MessageInput> rule)
        {
            moa_rules.Add(rule);
        }

        public void RemoveRule(ARX_Rule<T_MessageInput> rule)
        {
            moa_rules.Remove(rule);
        }


        /// <summary>
        /// Function runs each of this ruleset's rules on a given input.
        /// </summary>
        /// <param name="oInput"></param>
        public void ValidateInput(T_MessageInput oInput)
        {
            bool bPassMessageOn = true;

            foreach (ARX_Rule<T_MessageInput> rule in moa_rules)
            {
                if (rule.Condition(oInput) == true)
                {
                    bPassMessageOn = rule.PositiveAction();
                }
                else
                {
                    bPassMessageOn = rule.NegativeAction();
                }

                if (!bPassMessageOn)
                {
                    ConsumeInput(oInput);
                    return;
                }
            }

            PassInputOn(oInput);
        }
    }

}
