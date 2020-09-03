import { LanguageActionTypeEnum } from '../../../app/stores/aspian-core/locale/types';

/**
 * Converts English numbers to Persian.
 *
 * @param n is a number you want to convert to Persian.
 * @returns Persian number. Please notice that the retuning type is string.
 */
export const e2p = (n: string | number) =>
  n.toString().replace(/\d/g, (d: string) => '۰۱۲۳۴۵۶۷۸۹'[Number(d)]);

/**
 * Converts English numbers to Arabic.
 *
 * @param n is a number you want to convert to Arabic.
 * @returns Arabic number. Please notice that the retuning type is string.
 */
export const e2a = (n: string | number) =>
  n.toString().replace(/\d/g, (d: string) => '٠١٢٣٤٥٦٧٨٩'[Number(d)]);

/**
 * Converts Persian numbers to English.
 *
 * @param s is a Persian number you want to convert to English.
 * @returns English number. Please notice that the retuning type is number.
 */
export const p2e = (s: string) =>
  Number(
    s.replace(/[۰-۹]/g, (d: string) => '۰۱۲۳۴۵۶۷۸۹'.indexOf(d).toString())
  );

/**
 * Converts Arabic numbers to English.
 *
 * @param s is an Arabic number you want to convert to English.
 * @returns English number. Please notice that the retuning type is number.
 */
export const a2e = (s: string) =>
  Number(
    s.replace(/[٠-٩]/g, (d: string) => '٠١٢٣٤٥٦٧٨٩'.indexOf(d).toString())
  );

/**
 * Converts Persian numbers to Arabic.
 *
 * @param n is a Persian number you want to convert to Arabic.
 * @returns Arabic number. Please notice that the retuning type is string.
 */
export const p2a = (s: string) =>
  s.replace(/[۰-۹]/g, (d) => '٠١٢٣٤٥٦٧٨٩'['۰۱۲۳۴۵۶۷۸۹'.indexOf(d)]);

/**
 * Converts Arabic numbers to Persian.
 *
 * @param n is an Arabic number you want to convert to Persian.
 * @returns Persian number. Please notice that the retuning type is string.
 */
export const a2p = (s: string) =>
  s.replace(/[٠-٩]/g, (d) => '۰۱۲۳۴۵۶۷۸۹'['٠١٢٣٤٥٦٧٨٩'.indexOf(d)]);

/**
 * Converts numbers to the current language the page or environment is using.
 * @param number is a numbet you want to convert to current language.
 * @param numberLang of type of LanguageActionTypeEnum is the language of number (e.g. English number, Arabic number etc.)
 * @param currentLang of type of LanguageActionTypeEnum is the current language of the current environment or page.
 * @returns an appropriate number based on current language.
 */
export const ConvertDigitsToCurrentLanguage = (
  number: string | number,
  numberLang: LanguageActionTypeEnum,
  currentLang: LanguageActionTypeEnum
) => {
  switch (currentLang) {
    case LanguageActionTypeEnum.en:
      switch (numberLang) {
        case LanguageActionTypeEnum.fa:
          return p2e(number.toString());

        default:
          return number;
      }
    case LanguageActionTypeEnum.fa:
      switch (numberLang) {
        case LanguageActionTypeEnum.en:
          return e2p(number.toString());

        default:
          return number;
      }

    default:
      return number;
  }
};

/**
 * Converts numbers from languages except English which you want to convert to a standard English numbers.
 * @param number is a number of type of string from languages other than English which you want to convert to a standard English number.
 * @returns a standard English number.
 */
export const ConvertToStandardNumber = (number: string | number) => {
  const reg_arabicNumbers = /[\u0660-\u06690-9]+/;
  const reg_persianNumbers = /[\u06F0-\u06F90-9]+/;

  const isArabic = reg_arabicNumbers.test(number.toString());
  const isPersian = reg_persianNumbers.test(number.toString());

  if ((isArabic && !isPersian) || (isPersian && !isArabic)) {
    if (isArabic) return a2e(number.toString());
    if (isPersian) return p2e(number.toString());
  }

  return number;
};
