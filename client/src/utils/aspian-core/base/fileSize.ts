import { LanguageActionTypeEnum } from '../../../app/stores/aspian-core/locale/types';
import { e2p } from './NumberConverter';

/**
 * Gets a file size number rounded to keep only two decimals and add an appropriate unit of file size to it.
 * @param fileSizeToBytes is raw file size you want to get rounded and with an appropriate unit added to it.
 * @param lang of type of LanguageActionTypeEnum is current selected language.
 * @returns a string including a file size with at most two decimals and an appropriate unit added to it.
 */
export const GetRoundedFileSize = (
  fileSizeToBytes: number,
  lang: LanguageActionTypeEnum = LanguageActionTypeEnum.en
) => {
  let roundedFileSize: number = fileSizeToBytes;

  if (lang === LanguageActionTypeEnum.en) {
    if (roundedFileSize < 1024) return `${roundedFileSize} bytes`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${roundedFileSize} KB`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${roundedFileSize} MB`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${roundedFileSize} GB`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${roundedFileSize} TB`;
  }

  if (lang === LanguageActionTypeEnum.fa) {
    if (roundedFileSize < 1024) return `${e2p(roundedFileSize)} یایت`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${e2p(roundedFileSize)} کیلوبایت`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${e2p(roundedFileSize)} مگابایت`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${e2p(roundedFileSize)} گیگابایت`;

    if (roundedFileSize >= 1024)
      roundedFileSize = parseFloat((roundedFileSize / 1024).toFixed(2));
    if (roundedFileSize < 1024) return `${e2p(roundedFileSize)} ترابایت`;
  }

  return `${fileSizeToBytes} bytes`;
};
